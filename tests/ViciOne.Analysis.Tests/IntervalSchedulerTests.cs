using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AwesomeAssertions;
using ViciOne.Analysis;
using Xunit;

public class IntervalSchedulerTests
{
    public class RunDueGroups
    {
        private readonly ManualTimeProvider _time = new();
        private readonly IntervalScheduler _scheduler;

        public RunDueGroups()
            => _scheduler = new IntervalScheduler(_time, false);

        [Fact]
        public void TicksGroupOnlyWhenDue()
        {
            CountingIntervalElapsedHandler handler = new();
            using IntervalRegistration registration = _scheduler.Register(100, handler);

            _time.Advance(99);
            _scheduler.RunDueGroups();
            handler.Count.Should().Be(0);

            _time.Advance(1);
            _scheduler.RunDueGroups();
            handler.Count.Should().Be(1);
        }

        [Fact]
        public void KeepsFixedRateWithoutDrift()
        {
            CountingIntervalElapsedHandler handler = new();
            using IntervalRegistration registration = _scheduler.Register(100, handler);

            _time.Advance(130);
            _scheduler.RunDueGroups();
            _time.Advance(70);
            _scheduler.RunDueGroups();

            handler.Count.Should().Be(2);
        }

        [Fact]
        public void TicksRemainingMembersIfHandlerThrows()
        {
            CountingIntervalElapsedHandler handler = new();
            using IntervalRegistration failing = _scheduler.Register(100, new ThrowingIntervalElapsedHandler());
            using IntervalRegistration registration = _scheduler.Register(100, handler);

            for (int i = 0; i < 2; i++)
            {
                _time.Advance(100);
                _scheduler.RunDueGroups();
            }

            handler.Count.Should().Be(2);
            failing.ErrorReported.Should().BeTrue();
        }

        [Fact]
        public void RephasesAfterStall()
        {
            using IntervalRegistration registration = _scheduler.Register(100, new CountingIntervalElapsedHandler());

            _time.Advance(350);

            _scheduler.RunDueGroups().Should().Be(_time.GetTimestamp() + 100);
        }

        [Fact]
        public void TicksAgainAfterGroupWasRemovedAndRegisteredAgain()
        {
            _scheduler.Register(100, new CountingIntervalElapsedHandler()).Dispose();
            CountingIntervalElapsedHandler handler = new();
            using IntervalRegistration registration = _scheduler.Register(100, handler);

            _time.Advance(100);
            _scheduler.RunDueGroups();

            handler.Count.Should().Be(1);
        }

        [Fact]
        public void ReportsMissedIntervalsWhenFallingBehind()
        {
            CountingIntervalElapsedHandler handler = new();
            using IntervalRegistration registration = _scheduler.Register(100, handler);

            _time.Advance(350);
            _scheduler.RunDueGroups();

            handler.Count.Should().Be(1);
            handler.MissedIntervals.Should().Be(2);
        }

        [Fact]
        public void TicksDifferentIntervalsIndependently()
        {
            CountingIntervalElapsedHandler fast = new();
            CountingIntervalElapsedHandler slow = new();
            using IntervalRegistration fastRegistration = _scheduler.Register(100, fast);
            using IntervalRegistration slowRegistration = _scheduler.Register(300, slow);

            for (int i = 0; i < 3; i++)
            {
                _time.Advance(100);
                _scheduler.RunDueGroups();
            }

            fast.Count.Should().Be(3);
            slow.Count.Should().Be(1);
        }

        [Fact]
        public void KeepsRemainingMembersAfterUnregister()
        {
            List<CountingIntervalElapsedHandler> handlers = [];
            List<IntervalRegistration> registrations = [];
            for (int i = 0; i < 5; i++)
            {
                CountingIntervalElapsedHandler handler = new();
                handlers.Add(handler);
                registrations.Add(_scheduler.Register(100, handler));
            }

            registrations[1].Dispose();
            registrations[1].Dispose();
            registrations[3].Dispose();
            _time.Advance(100);
            _scheduler.RunDueGroups();

            handlers.ConvertAll(x => x.Count).Should().Equal(1, 0, 1, 0, 1);
        }

        [Fact]
        public void RemovesEmptyGroup()
        {
            IntervalRegistration registration = _scheduler.Register(100, new CountingIntervalElapsedHandler());

            registration.Dispose();

            _scheduler.RunDueGroups().Should().Be(long.MaxValue);
        }
    }

    public class Worker
    {
        private static bool WaitUntil(Func<bool> condition)
        {
            Stopwatch timeout = Stopwatch.StartNew();
            while (timeout.Elapsed < TimeSpan.FromSeconds(5))
            {
                if (condition())
                    return true;
                Thread.Sleep(10);
            }

            return condition();
        }

        [Fact]
        public void EndsWhenLastRegistrationIsDisposed()
        {
            IntervalScheduler scheduler = new(new ManualTimeProvider(), true);
            IntervalRegistration registration = scheduler.Register(60_000, new CountingIntervalElapsedHandler());
            scheduler.IsThreadRunning.Should().BeTrue();

            registration.Dispose();

            WaitUntil(() => scheduler.IsThreadRunning == false).Should().BeTrue();
        }

        [Fact]
        public void StartsAgainOnNextRegister()
        {
            IntervalScheduler scheduler = new(new ManualTimeProvider(), true);
            scheduler.Register(60_000, new CountingIntervalElapsedHandler()).Dispose();
            WaitUntil(() => scheduler.IsThreadRunning == false).Should().BeTrue();

            using IntervalRegistration registration = scheduler.Register(60_000, new CountingIntervalElapsedHandler());

            scheduler.IsThreadRunning.Should().BeTrue();
        }

        [Fact]
        public void EndsWhenCleared()
        {
            IntervalScheduler scheduler = new(new ManualTimeProvider(), true);
            using IntervalRegistration registration = scheduler.Register(60_000, new CountingIntervalElapsedHandler());

            scheduler.Clear();

            WaitUntil(() => scheduler.IsThreadRunning == false).Should().BeTrue();
        }

        [Fact]
        public void TicksWithSystemTimeWhileRegistrationsChangeConcurrently()
        {
            IntervalScheduler scheduler = new(TimeProvider.System, true);
            CountingIntervalElapsedHandler steady = new();
            IntervalRegistration steadyRegistration = scheduler.Register(10, steady);

            Parallel.For(0, 8, worker =>
            {
                for (int i = 0; i < 200; i++)
                    scheduler.Register(1 + ((worker + i) % 5), new CountingIntervalElapsedHandler()).Dispose();
            });

            WaitUntil(() => steady.Count >= 3).Should().BeTrue();
            steadyRegistration.Dispose();
            WaitUntil(() => scheduler.IsThreadRunning == false).Should().BeTrue();
            scheduler.RunDueGroups().Should().Be(long.MaxValue);
        }

        [Fact]
        public void KeepsTickingAfterException()
        {
            IntervalScheduler scheduler = new(new FailingOnceTimeProvider(), true);
            CountingIntervalElapsedHandler handler = new();
            using IntervalRegistration registration = scheduler.Register(10, handler);

            WaitUntil(() => handler.Count > 0).Should().BeTrue();
            scheduler.IsThreadRunning.Should().BeTrue();
        }

        /// <summary>
        /// Real time that throws on the first timestamp read by the worker (the second read overall).
        /// </summary>
        private sealed class FailingOnceTimeProvider : TimeProvider
        {
            private int _calls;

            public override long GetTimestamp()
                => Interlocked.Increment(ref _calls) == 2
                    ? throw new InvalidOperationException("Simulated failure")
                    : Stopwatch.GetTimestamp();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
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
            var handler = new CountingIntervalElapsedHandler();
            using var registration = _scheduler.Register(100, handler);

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
            var handler = new CountingIntervalElapsedHandler();
            using var registration = _scheduler.Register(100, handler);

            _time.Advance(130);
            _scheduler.RunDueGroups();
            _time.Advance(70);
            _scheduler.RunDueGroups();

            handler.Count.Should().Be(2);
        }

        [Fact]
        public void TicksRemainingMembersIfHandlerThrows()
        {
            var handler = new CountingIntervalElapsedHandler();
            using var failing = _scheduler.Register(100, new ThrowingIntervalElapsedHandler());
            using var registration = _scheduler.Register(100, handler);

            for (var i = 0; i < 2; i++)
            {
                _time.Advance(100);
                _scheduler.RunDueGroups();
            }

            handler.Count.Should().Be(2);
            failing.ErrorReported.Should().BeTrue();
        }

        [Fact]
        public void ReportsMissedIntervalsWhenFallingBehind()
        {
            var handler = new CountingIntervalElapsedHandler();
            using var registration = _scheduler.Register(100, handler);

            _time.Advance(350);
            _scheduler.RunDueGroups();

            handler.Count.Should().Be(1);
            handler.MissedIntervals.Should().Be(2);
        }

        [Fact]
        public void TicksDifferentIntervalsIndependently()
        {
            var fast = new CountingIntervalElapsedHandler();
            var slow = new CountingIntervalElapsedHandler();
            using var fastRegistration = _scheduler.Register(100, fast);
            using var slowRegistration = _scheduler.Register(300, slow);

            for (var i = 0; i < 3; i++)
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
            var handlers = new List<CountingIntervalElapsedHandler>();
            var registrations = new List<IntervalRegistration>();
            for (var i = 0; i < 5; i++)
            {
                var handler = new CountingIntervalElapsedHandler();
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
            var registration = _scheduler.Register(100, new CountingIntervalElapsedHandler());

            registration.Dispose();

            _scheduler.RunDueGroups().Should().Be(long.MaxValue);
        }
    }

    public class Worker
    {
        private static bool WaitUntil(Func<bool> condition)
        {
            var timeout = Stopwatch.StartNew();
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
            var scheduler = new IntervalScheduler(new ManualTimeProvider(), true);
            var registration = scheduler.Register(60_000, new CountingIntervalElapsedHandler());
            scheduler.IsThreadRunning.Should().BeTrue();

            registration.Dispose();

            WaitUntil(() => scheduler.IsThreadRunning == false).Should().BeTrue();
        }

        [Fact]
        public void StartsAgainOnNextRegister()
        {
            var scheduler = new IntervalScheduler(new ManualTimeProvider(), true);
            scheduler.Register(60_000, new CountingIntervalElapsedHandler()).Dispose();
            WaitUntil(() => scheduler.IsThreadRunning == false).Should().BeTrue();

            using var registration = scheduler.Register(60_000, new CountingIntervalElapsedHandler());

            scheduler.IsThreadRunning.Should().BeTrue();
        }

        [Fact]
        public void EndsWhenCleared()
        {
            var scheduler = new IntervalScheduler(new ManualTimeProvider(), true);
            using var registration = scheduler.Register(60_000, new CountingIntervalElapsedHandler());

            scheduler.Clear();

            WaitUntil(() => scheduler.IsThreadRunning == false).Should().BeTrue();
        }

        [Fact]
        public void KeepsTickingAfterException()
        {
            var scheduler = new IntervalScheduler(new FailingOnceTimeProvider(), true);
            var handler = new CountingIntervalElapsedHandler();
            using var registration = scheduler.Register(10, handler);

            WaitUntil(() => handler.Count > 0).Should().BeTrue();
            scheduler.IsThreadRunning.Should().BeTrue();
        }

        /// <summary>
        ///     Real time that throws on the first timestamp read by the worker (the second read overall).
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

using System.Collections.Generic;
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
}

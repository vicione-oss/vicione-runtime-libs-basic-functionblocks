using System;
using AwesomeAssertions;
using ViciOne.Time;
using Xunit;

public class SchedulerTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<Scheduler>
    {
        [Fact]
        public void SetsAndResetsTriggerOutputIfCurrentTimeIsScheduled()
        {
            var now = DateTime.Now;
            ChangeSettingValue(nameof(Scheduler.Settings.Times), $"{(int)now.DayOfWeek}#{now.Hour:D2}:{now.Minute:D2}");
            StartEngine();
            var scheduler = GetRuntimeFunctionBlock();
            scheduler.DeactivateTimer();
            scheduler.CheckScheduler(now);

            ProcessCycle();
            var outputAfterFirstCycle = GetOutputValue<bool>(nameof(Scheduler.ProcessDataOutputs.Trigger));
            ProcessCycle();
            var outputAfterSecondCycle = GetOutputValue<bool>(nameof(Scheduler.ProcessDataOutputs.Trigger));

            outputAfterFirstCycle.Should().BeTrue();
            outputAfterSecondCycle.Should().BeFalse();
        }

        [Fact]
        public void DoesNotSetTriggerOutputIfCurrentTimeIsNotScheduled()
        {
            var now = DateTime.Now;
            var tomorrow = now.AddDays(1);
            ChangeSettingValue(nameof(Scheduler.Settings.Times), $"{(int)tomorrow.DayOfWeek}#{tomorrow.Hour:D2}:{tomorrow.Minute:D2}");
            StartEngine();
            var scheduler = GetRuntimeFunctionBlock();
            scheduler.DeactivateTimer();
            scheduler.CheckScheduler(now);

            ProcessCycle();
            var outputAfterFirstCycle = GetOutputValue<bool>(nameof(Scheduler.ProcessDataOutputs.Trigger));
            ProcessCycle();
            var outputAfterSecondCycle = GetOutputValue<bool>(nameof(Scheduler.ProcessDataOutputs.Trigger));

            outputAfterFirstCycle.Should().BeFalse();
            outputAfterSecondCycle.Should().BeFalse();
        }

        [Fact]
        public void DoesNotSetTriggerOutputIfDisabled()
        {
            var now = DateTime.Now;
            ChangeSettingValue(nameof(Scheduler.Settings.Times), $"{(int)now.DayOfWeek}#{now.Hour:D2}:{now.Minute:D2}");
            ChangeEnabled(false);
            StartEngine();
            var scheduler = GetRuntimeFunctionBlock();
            scheduler.DeactivateTimer();
            scheduler.CheckScheduler(now);

            ProcessCycle();
            var outputAfterFirstCycle = GetOutputValue<bool>(nameof(Scheduler.ProcessDataOutputs.Trigger));
            ProcessCycle();
            var outputAfterSecondCycle = GetOutputValue<bool>(nameof(Scheduler.ProcessDataOutputs.Trigger));

            outputAfterFirstCycle.Should().BeFalse();
            outputAfterSecondCycle.Should().BeFalse();
        }
    }
}

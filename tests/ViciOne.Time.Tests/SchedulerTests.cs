using System;
using AwesomeAssertions;
using ViciOne.Time;
using Xunit;

public class SchedulerTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<Scheduler>
    {
        protected override bool AllowErrorsInProcessCycle => true;

        [Fact]
        public void OutputIsTrueIfCurrentTimeIsScheduled()
        {
            var now = DateTime.Now;
            ChangeSettingValue(nameof(Scheduler.Settings.Times), $"{(int)now.DayOfWeek}#{now.Hour:D2}:{now.Minute:D2}");
            StartEngine();
            // TODO: Currently we need a ProcessCycle to ensure the settings are applied. With Core v1.0.0 we need are able to apply settings immediately, so we can remove this ProcessCycle.
            ProcessCycle();
            var scheduler = GetRuntimeFunctionBlock();
            scheduler.DeactivateTimer();
            scheduler.CheckScheduler(now);

            ProcessCycle();

            GetOutputValue<bool>(nameof(Scheduler.ProcessDataOutputs.Trigger)).Should().BeTrue();

            ProcessCycle();

            GetOutputValue<bool>(nameof(Scheduler.ProcessDataOutputs.Trigger)).Should().BeFalse();
        }

        [Fact]
        public void OutputsFalseIfCurrentTimeIsNotScheduled()
        {
            var now = DateTime.Now;
            StartEngine();
            // TODO: Currently we need a ProcessCycle to ensure the settings are applied. With Core v1.0.0 we need are able to apply settings immediately, so we can remove this ProcessCycle.
            ProcessCycle();
            var scheduler = GetRuntimeFunctionBlock();
            scheduler.DeactivateTimer();
            scheduler.CheckScheduler(now);

            ProcessCycle();

            GetOutputValue<bool>(nameof(Scheduler.ProcessDataOutputs.Trigger)).Should().BeFalse();

            ProcessCycle();

            GetOutputValue<bool>(nameof(Scheduler.ProcessDataOutputs.Trigger)).Should().BeFalse();
        }

        [Fact]
        public void DoesNotProcessCycleIfDisabled()
        {
            var now = DateTime.Now;
            ChangeSettingValue(nameof(Scheduler.Settings.Times), $"{(int)now.DayOfWeek}#{now.Hour:D2}:{now.Minute:D2}");
            ChangeEnabled(false);
            StartEngine();
            // TODO: Currently we need a ProcessCycle to ensure the settings are applied. With Core v1.0.0 we need are able to apply settings immediately, so we can remove this ProcessCycle.
            ProcessCycle();
            var scheduler = GetRuntimeFunctionBlock();
            scheduler.DeactivateTimer();
            scheduler.CheckScheduler(now);

            ProcessCycle();

            GetOutputValue<bool>(nameof(Scheduler.ProcessDataOutputs.Trigger)).Should().BeFalse();

            ProcessCycle();

            GetOutputValue<bool>(nameof(Scheduler.ProcessDataOutputs.Trigger)).Should().BeFalse();
        }
    }
}

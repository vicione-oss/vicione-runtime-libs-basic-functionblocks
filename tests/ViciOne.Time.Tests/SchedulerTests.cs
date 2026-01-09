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
            var scheduler = GetRuntimeFunctionBlock();
            var now = DateTime.Now;

            scheduler.DeactivateTimer();
            ChangeSettingValue(nameof(Scheduler.Settings.Times), $"{(int)now.DayOfWeek}#{now.Hour:D2}:{now.Minute:D2}");
            ProcessCycle();
            scheduler.CheckScheduler(now);

            ProcessCycle();

            GetOutputValue<bool>(nameof(Scheduler.ProcessDataOutputs.Trigger)).Should().BeTrue();

            ProcessCycle();

            GetOutputValue<bool>(nameof(Scheduler.ProcessDataOutputs.Trigger)).Should().BeFalse();
        }

        [Fact]
        public void OutputsFalseIfCurrentTimeIsNotScheduled()
        {
            var scheduler = GetRuntimeFunctionBlock();
            var now = DateTime.Now;

            scheduler.DeactivateTimer();
            ProcessCycle();
            scheduler.CheckScheduler(now);

            ProcessCycle();

            GetOutputValue<bool>(nameof(Scheduler.ProcessDataOutputs.Trigger)).Should().BeFalse();

            ProcessCycle();

            GetOutputValue<bool>(nameof(Scheduler.ProcessDataOutputs.Trigger)).Should().BeFalse();
        }

        [Fact]
        public void DoseNotProcessCycleWhenDisabled()
        {
            var scheduler = GetRuntimeFunctionBlock();
            var now = DateTime.Now;

            ChangeEnabled(false);
            scheduler.DeactivateTimer();
            ChangeSettingValue(nameof(Scheduler.Settings.Times), $"{(int)now.DayOfWeek}#{now.Hour:D2}:{now.Minute:D2}");
            ProcessCycle();
            scheduler.CheckScheduler(now);

            ProcessCycle();

            GetOutputValue<bool>(nameof(Scheduler.ProcessDataOutputs.Trigger)).Should().BeFalse();

            ProcessCycle();

            GetOutputValue<bool>(nameof(Scheduler.ProcessDataOutputs.Trigger)).Should().BeFalse();
        }
    }
}

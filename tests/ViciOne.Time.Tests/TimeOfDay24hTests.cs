using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using AwesomeAssertions;
using AwesomeAssertions.Extensions;
using ViciOne.Core.Dataflow.DataModel;
using ViciOne.Core.Runtime.Execution;
using ViciOne.Time;
using Xunit;

public class TimeOfDay24hTests
{
    public class GetNow
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetsCorrectResult(bool useUtc)
        {
            var expected = useUtc ? DateTime.UtcNow : DateTime.Now;

            var result = TimeOfDay24h.GetNow(useUtc);

            result.Should().BeIn(expected.Kind)
                .And.BeCloseTo(expected, 1.Seconds());
        }
    }

    public class OnSettingChanged : RuntimeFunctionBlockTests<TimeOfDay24h>
    {
        [RunOnlyIfTimezoneIsNotUtcFact]
        public void SetsOutputIfUseUtcChanged()
        {
            SetRunMode(ViciOne.Core.Contracts.DataModel.RunMode.Cyclic, uint.MaxValue);
            ProcessCycle();
            var outputHourBefore = GetOutputValue<double>(nameof(TimeOfDay24h.ProcessDataOutputs.Hour));
            ChangeSettingValue(nameof(TimeOfDay24h.Settings.UseUtc), true);

            ProcessCycle();

            var outputHour = GetOutputValue<double>(nameof(TimeOfDay24h.ProcessDataOutputs.Hour));
            outputHour.Should().NotBe(outputHourBefore);
        }

        [RunOnlyIfTimezoneIsNotUtcFact]
        public void DoesNotSetOutputIfBlockIsNotEnabled()
        {
            SetRunMode(ViciOne.Core.Contracts.DataModel.RunMode.Cyclic, uint.MaxValue);
            ProcessCycle();
            ChangeEnabled(false);
            ProcessCycle();
            var outputHourBefore = GetOutputValue<double>(nameof(TimeOfDay24h.ProcessDataOutputs.Hour));
            ChangeSettingValue(nameof(TimeOfDay24h.Settings.UseUtc), true);

            ProcessCycle();

            var outputHour = GetOutputValue<double>(nameof(TimeOfDay24h.ProcessDataOutputs.Hour));
            outputHour.Should().Be(outputHourBefore);
        }

        private static Dataflow ExtractDataflow(DataflowInfo dataflowInfo)
            => dataflowInfo.Dataflow as Dataflow ?? throw new InvalidOperationException("These test cases can only be executed if Core data model is used.");

        private void SetRunMode(ViciOne.Core.Contracts.DataModel.RunMode runMode, uint cycleFrequency = 1)
        {
            var block = ExtractDataflow(DataflowInfo).RootContainer.FunctionBlocks.First();
            block.RunMode = runMode;
            block.CycleFrequency = cycleFrequency;
        }

        public sealed class RunOnlyIfTimezoneIsNotUtcFact : FactAttribute
        {
            public RunOnlyIfTimezoneIsNotUtcFact()
            {
                if (TimeZoneInfo.Local.BaseUtcOffset.Hours == 0)
                    Skip = "Ignore if time difference to UTC is 0 hours because a change of setting 'UseUtc' has no verifiable effect.";
            }
        }
    }

    public class ProcessCycle : RuntimeFunctionBlockTests<TimeOfDay24h>
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CalculatesCorrectResults(bool useUtc)
        {
            ChangeSettingValue(nameof(TimeOfDay24h.Settings.UseUtc), useUtc);
            ProcessCycle();
            var timeBefore = DateTime.Now.Date.Add(new TimeSpan(
                (int)GetOutputValue<double>(nameof(TimeOfDay24h.ProcessDataOutputs.Hour)),
                (int)GetOutputValue<double>(nameof(TimeOfDay24h.ProcessDataOutputs.Minute)),
                (int)GetOutputValue<double>(nameof(TimeOfDay24h.ProcessDataOutputs.Second))));

            ProcessCycle();

            var timeAfter = timeBefore.Date.Add(new TimeSpan(
                (int)GetOutputValue<double>(nameof(TimeOfDay24h.ProcessDataOutputs.Hour)),
                (int)GetOutputValue<double>(nameof(TimeOfDay24h.ProcessDataOutputs.Minute)),
                (int)GetOutputValue<double>(nameof(TimeOfDay24h.ProcessDataOutputs.Second))));
            if (timeAfter < timeBefore)
                timeAfter = timeAfter.AddDays(1);
            timeAfter.Should().BeCloseTo(timeBefore, 2.Seconds());
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "CA1508:Toten bedingten Code vermeiden", Justification = "outputValuesChanged wird im Lambda-Event-Handler gesetzt")]
        public void CalculatesNewResults()
        {
            var outputSecondBefore = GetOutputValue<double>(nameof(TimeOfDay24h.ProcessDataOutputs.Second));
            var outputValuesChanged = false;
            StartEngine();
            BlockExecutor!.OutputValuesChanged += (_, _) => outputValuesChanged = true;
            var watch = Stopwatch.StartNew();

            do
            {
                ProcessCycle();
                Thread.Sleep(1);
            }
            while (outputValuesChanged is false && watch.ElapsedMilliseconds < 1200);

            var outputSecondAfter = GetOutputValue<double>(nameof(TimeOfDay24h.ProcessDataOutputs.Second));
            outputSecondAfter.Should().NotBe(outputSecondBefore);
        }

        [Fact]
        public void DoesNotProcessOutputIfBlockIsNotEnabled()
        {
            var outputSecondBefore = GetOutputValue<double>(nameof(TimeOfDay24h.ProcessDataOutputs.Second));
            ChangeEnabled(false);

            ProcessCycle();

            var outputSecondAfter = GetOutputValue<double>(nameof(TimeOfDay24h.ProcessDataOutputs.Second));
            outputSecondAfter.Should().Be(outputSecondBefore);
        }

        [Fact]
        public void DoesNotProcessSettingIfBlockIsNotEnabled()
        {
            ChangeEnabled(false);
            ProcessCycle();
            var outputSecondBefore = GetOutputValue<double>(nameof(TimeOfDay24h.ProcessDataOutputs.Second));
            ChangeSettingValue(nameof(TimeOfDay24h.Settings.UseUtc), true);

            ProcessCycle();

            var outputSecondAfter = GetOutputValue<double>(nameof(TimeOfDay24h.ProcessDataOutputs.Second));
            outputSecondAfter.Should().Be(outputSecondBefore);
        }
    }
}

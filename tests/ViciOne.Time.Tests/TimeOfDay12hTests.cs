using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using AwesomeAssertions;
using AwesomeAssertions.Extensions;
using ViciOne.Core.Dataflow.DataModel;
using ViciOne.Core.Runtime.Execution;
using ViciOne.Time;
using Xunit;

public class TimeOfDay12hTests
{
    public class GetAmPmValue
    {
        [Theory]
        [InlineData(11, 59, 59, 0)]
        [InlineData(0, 0, 0, 0)]
        [InlineData(12, 0, 0, 1)]
        [InlineData(23, 59, 59, 1)]
        public void CalculatesAmOrPmCorrect(int hour, int minute, int second, int expectedValue)
        {
            var dateTime = new DateTime(2021, 4, 27, hour, minute, second);

            var calculatedValue = TimeOfDay12h.GetAmPmValue(dateTime);

            calculatedValue.Should().Be(expectedValue);
        }
    }

    public class GetHour12h
    {
        [Theory]
        [InlineData(11, 59, 59, 11)]
        [InlineData(0, 0, 0, 12)]
        [InlineData(12, 0, 0, 12)]
        [InlineData(23, 59, 59, 11)]
        [InlineData(15, 23, 59, 3)]
        [InlineData(6, 23, 59, 6)]
        public void CalculatesHour12hCorrect(int hour, int minute, int second, int expectedValue)
        {
            var dateTime = new DateTime(2021, 4, 27, hour, minute, second);

            var calculatedValue = TimeOfDay12h.GetHour12h(dateTime);

            calculatedValue.Should().Be(expectedValue);
        }
    }

    public class GetNow
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetsCorrectResult(bool useUtc)
        {
            var expected = useUtc ? DateTime.UtcNow : DateTime.Now;

            var result = TimeOfDay12h.GetNow(useUtc);

            result.Should().BeIn(expected.Kind)
                .And.BeCloseTo(expected, 1.Seconds());
        }
    }

    public class OnSettingChanged() : RuntimeFunctionBlockTests<TimeOfDay12h>(GenerateDataflow())
    {
        [RunOnlyIfTimezoneConditionsMetFact]
        public void SetsOutputIfUseUtcChanged()
        {
            var outputHourBefore = GetOutputValue<double>(nameof(TimeOfDay12h.ProcessDataOutputs.Hour));
            ChangeSettingValue(nameof(TimeOfDay12h.Settings.UseUtc), true);

            ProcessCycle();

            var outputHour = GetOutputValue<double>(nameof(TimeOfDay12h.ProcessDataOutputs.Hour));
            outputHour.Should().NotBe(outputHourBefore);
        }

        [RunOnlyIfTimezoneConditionsMetFact]
        public void DoesNotSetOutputIfBlockIsNotEnabled()
        {
            ChangeEnabled(false);
            ProcessCycle();
            var outputHourBefore = GetOutputValue<double>(nameof(TimeOfDay12h.ProcessDataOutputs.Hour));
            ChangeSettingValue(nameof(TimeOfDay12h.Settings.UseUtc), true);

            ProcessCycle();

            var outputHour = GetOutputValue<double>(nameof(TimeOfDay12h.ProcessDataOutputs.Hour));
            outputHour.Should().Be(outputHourBefore);
        }

        private static Dataflow ExtractDataflow(DataflowInfo dataflowInfo)
            => dataflowInfo.Dataflow as Dataflow ?? throw new InvalidOperationException("These test cases can only be executed if Core data model is used.");

        private static (DataflowInfo, IEnumerable<Assembly>) GenerateDataflow()
        {
            var assemblies = CreateDefaultAssemblies();
            var info = GenerateDefaultDataflow(assemblies);
            var dataflow = ExtractDataflow(info);
            var block = dataflow.RootContainer.FunctionBlocks.Single();
            block.RunMode = ViciOne.Core.Contracts.DataModel.RunMode.Cyclic;
            block.CycleFrequency = uint.MaxValue;
            return (info, assemblies.AllAssemblies);
        }

        public sealed class RunOnlyIfTimezoneConditionsMetFact : FactAttribute
        {
            public RunOnlyIfTimezoneConditionsMetFact()
            {
                var baseUtcOffset = TimeZoneInfo.Local.BaseUtcOffset;
                if (Math.Abs(baseUtcOffset.Hours) is 0 or 12)
                    Skip = "Ignore if time difference to UTC is 0 or 12 hours because a change of setting 'UseUtc' has no verifiable effect.";
            }
        }
    }

    public class ProcessCycle : RuntimeFunctionBlockTests<TimeOfDay12h>
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CalculatesCorrectResults(bool useUtc)
        {
            ChangeSettingValue(nameof(TimeOfDay12h.Settings.UseUtc), useUtc);
            ProcessCycle();
            var timeBefore = DateTime.Now.Date.Add(new TimeSpan(
                (int)GetOutputValue<double>(nameof(TimeOfDay12h.ProcessDataOutputs.Hour)),
                (int)GetOutputValue<double>(nameof(TimeOfDay12h.ProcessDataOutputs.Minute)),
                (int)GetOutputValue<double>(nameof(TimeOfDay12h.ProcessDataOutputs.Second))));

            ProcessCycle();

            var timeAfter = timeBefore.Date.Add(new TimeSpan(
                (int)GetOutputValue<double>(nameof(TimeOfDay12h.ProcessDataOutputs.Hour)),
                (int)GetOutputValue<double>(nameof(TimeOfDay12h.ProcessDataOutputs.Minute)),
                (int)GetOutputValue<double>(nameof(TimeOfDay12h.ProcessDataOutputs.Second))));
            if (timeAfter < timeBefore)
                timeAfter = timeAfter.AddDays(1);
            timeAfter.Should().BeCloseTo(timeBefore, 2.Seconds());
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "CA1508:Toten bedingten Code vermeiden", Justification = "outputValuesChanged wird im Lambda-Event-Handler gesetzt")]
        public void CalculatesNewResults()
        {
            var outputSecondBefore = GetOutputValue<double>(nameof(TimeOfDay12h.ProcessDataOutputs.Second));
            ProcessCycle();
            var outputValuesChanged = false;
            BlockExecutor.OutputValuesChanged += (_, _) => outputValuesChanged = true;
            var watch = Stopwatch.StartNew();

            do
            {
                ProcessCycle();
                Thread.Sleep(1);
            }
            while (outputValuesChanged is false && watch.ElapsedMilliseconds < 1200);

            var outputSecondAfter = GetOutputValue<double>(nameof(TimeOfDay12h.ProcessDataOutputs.Second));
            outputSecondAfter.Should().NotBe(outputSecondBefore);
        }

        [Fact]
        public void DoesNotProcessOutputIfBlockIsNotEnabled()
        {
            var outputSecondBefore = GetOutputValue<double>(nameof(TimeOfDay12h.ProcessDataOutputs.Second));
            ChangeEnabled(false);

            ProcessCycle();

            var outputSecondAfter = GetOutputValue<double>(nameof(TimeOfDay12h.ProcessDataOutputs.Second));
            outputSecondAfter.Should().Be(outputSecondBefore);
        }

        [Fact]
        public void DoesNotProcessSettingIfBlockIsNotEnabled()
        {
            ChangeEnabled(false);
            ProcessCycle();
            var outputSecondBefore = GetOutputValue<double>(nameof(TimeOfDay12h.ProcessDataOutputs.Second));
            ChangeSettingValue(nameof(TimeOfDay12h.Settings.UseUtc), true);

            ProcessCycle();

            var outputSecondAfter = GetOutputValue<double>(nameof(TimeOfDay12h.ProcessDataOutputs.Second));
            outputSecondAfter.Should().Be(outputSecondBefore);
        }
    }
}

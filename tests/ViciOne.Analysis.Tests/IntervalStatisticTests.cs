using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using ACX.ViciOne.Data.Logger;
using AwesomeAssertions;
using ViciOne.Analysis;
using ViciOne.Core.Dataflow.DataModel;
using ViciOne.Core.Runtime.Execution;
using Xunit;

public class IntervalStatisticTests
{
    public class SetAveragingTimeToOutputs : RuntimeFunctionBlockTests<IntervalStatistic>
    {
        [Fact]
        public void DoesNothingIfBlockIsNotEnabled()
        {
            var average = 12.3;
            var difference = 23.4;
            var maximum = 34.5;
            var minimum = 45.6;
            var sum = 56.7;

            ChangeEnabled(false);
            ChangeInputValue(nameof(IntervalStatistic.ProcessDataInputs.Value), 42.0);
            ProcessCycle();

            var runtimeFunctionBlock = GetRuntimeFunctionBlock();
            runtimeFunctionBlock.SetAveragingTimeToOutputs(new(average, minimum, maximum, sum, difference));
            ProcessCycle();

            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Average)).Should().Be(0);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Difference)).Should().Be(0);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Maximum)).Should().Be(0);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Minimum)).Should().Be(0);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Sum)).Should().Be(0);
        }

        [Fact]
        public void DoesNothingIfInputWasNotChanged()
        {
            var average = 12.3;
            var difference = 23.4;
            var maximum = 34.5;
            var minimum = 45.6;
            var sum = 56.7;

            ProcessCycle();

            var runtimeFunctionBlock = GetRuntimeFunctionBlock();
            runtimeFunctionBlock.SetAveragingTimeToOutputs(new(average, minimum, maximum, sum, difference));
            ProcessCycle();

            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Average)).Should().Be(0);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Difference)).Should().Be(0);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Maximum)).Should().Be(0);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Minimum)).Should().Be(0);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Sum)).Should().Be(0);
        }

        [Fact]
        public void PassesInputValueToAverageOutputOnFirstCall()
        {
            var averageInputValue = 12.3;
            ChangeInputValue(nameof(IntervalStatistic.ProcessDataInputs.Value), averageInputValue);
            ProcessCycle();

            var runtimeFunctionBlock = GetRuntimeFunctionBlock();
            runtimeFunctionBlock.SetAveragingTimeToOutputs(new(42, default, default, default, default));
            ProcessCycle();

            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Average)).Should().Be(averageInputValue);
        }

        [Fact]
        public void SetsAveragingTimeExceptAverageToOutputsOnFirstCall()
        {
            var average = 12.3;
            var difference = 23.4;
            var maximum = 34.5;
            var minimum = 45.6;
            var sum = 56.7;

            ChangeInputValue(nameof(IntervalStatistic.ProcessDataInputs.Value), 42.0);
            ProcessCycle();

            var runtimeFunctionBlock = GetRuntimeFunctionBlock();
            runtimeFunctionBlock.SetAveragingTimeToOutputs(new(average, minimum, maximum, sum, difference));
            ProcessCycle();

            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Average)).Should().NotBe(average);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Difference)).Should().Be(difference);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Maximum)).Should().Be(maximum);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Minimum)).Should().Be(minimum);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Sum)).Should().Be(sum);
        }

        [Fact]
        public void SetsAveragingTimeToOutputsOnSecondCall()
        {
            var average = 12.3;
            var difference = 23.4;
            var maximum = 34.5;
            var minimum = 45.6;
            var sum = 56.7;

            ChangeInputValue(nameof(IntervalStatistic.ProcessDataInputs.Value), 42.0);
            ProcessCycle();
            var runtimeFunctionBlock = GetRuntimeFunctionBlock();
            runtimeFunctionBlock.SetAveragingTimeToOutputs(new(default, default, default, default, default));
            ProcessCycle();

            runtimeFunctionBlock.SetAveragingTimeToOutputs(new(average, minimum, maximum, sum, difference));
            ProcessCycle();

            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Average)).Should().Be(average);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Difference)).Should().Be(difference);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Maximum)).Should().Be(maximum);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Minimum)).Should().Be(minimum);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Sum)).Should().Be(sum);
        }
    }

    public class ProcessCycle : RuntimeFunctionBlockTests<IntervalStatistic>
    {
        [Fact]
        public void UpdatesOutputCorrectlyAfterTimeElapsed()
        {
            ChangeSettingValue(nameof(IntervalStatistic.Settings.CompressionTime), 200);
            ChangeInputValue(nameof(IntervalStatistic.ProcessDataInputs.Value), 2.0);
            ProcessCycle();
            var fb = GetRuntimeFunctionBlock();
            var weightedAverage = AverageExtractor.GetWeightedAverage(fb);
            using var waitHandle = new ManualResetEvent(false);
            AveragingTimeExpiredEventArgs? expectedValues = null;
            weightedAverage.AveragingTimeExpired += (_, e) =>
            {
                expectedValues = e;
                waitHandle.Set();
            };
            waitHandle.WaitOne(1000);

            ProcessCycle();

            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Average)).Should().Be(expectedValues!.Average);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Difference)).Should().Be(expectedValues.Difference);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Maximum)).Should().Be(expectedValues.Maximum);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Minimum)).Should().Be(expectedValues.Minimum);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Sum)).Should().Be(expectedValues.Sum);
        }

        [Fact]
        public void DoesNothingIfInputWasNotChanged()
        {
            ChangeSettingValue(nameof(IntervalStatistic.Settings.CompressionTime), 200);
            ProcessCycle();
            var fb = GetRuntimeFunctionBlock();

            var weightedAverage = AverageExtractor.GetWeightedAverage(fb);

            weightedAverage.Should().BeNull();
        }

        [Fact]
        public void UpdatesOutputCorrectlyAfterSecondCycle()
        {
            ChangeSettingValue(nameof(IntervalStatistic.Settings.CompressionTime), 200);
            ChangeInputValue(nameof(IntervalStatistic.ProcessDataInputs.Value), 2.0);

            ProcessCycle();
            var fb = GetRuntimeFunctionBlock();
            var weightedAverage = AverageExtractor.GetWeightedAverage(fb);
            using var waitHandle = new ManualResetEvent(false);
            AveragingTimeExpiredEventArgs? expectedValues = null;
            weightedAverage.AveragingTimeExpired += (_, e) =>
            {
                expectedValues = e;
                waitHandle.Set();
            };
            waitHandle.WaitOne(1000);

            ProcessCycle();

            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Average)).Should().Be(expectedValues!.Average);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Difference)).Should().Be(expectedValues.Difference);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Maximum)).Should().Be(expectedValues.Maximum);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Minimum)).Should().Be(expectedValues.Minimum);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Sum)).Should().Be(expectedValues.Sum);

            ChangeInputValue(nameof(IntervalStatistic.ProcessDataInputs.Value), 3.0);


            ProcessCycle();

            waitHandle.Reset();
            waitHandle.WaitOne(1000);

            ProcessCycle();

            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Average)).Should().Be(expectedValues.Average);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Difference)).Should().Be(expectedValues.Difference);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Maximum)).Should().Be(expectedValues.Maximum);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Minimum)).Should().Be(expectedValues.Minimum);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Sum)).Should().Be(expectedValues.Sum);
        }

        [Fact]
        public void UpdatesOutputCorrectlyWithNegativeValue()
        {
            ChangeSettingValue(nameof(IntervalStatistic.Settings.CompressionTime), 200);
            ChangeInputValue(nameof(IntervalStatistic.ProcessDataInputs.Value), -1.0);

            ProcessCycle();
            var fb = GetRuntimeFunctionBlock();
            var weightedAverage = AverageExtractor.GetWeightedAverage(fb);
            using var waitHandle = new ManualResetEvent(false);
            AveragingTimeExpiredEventArgs? expectedValues = null;
            weightedAverage.AveragingTimeExpired += (_, e) =>
            {
                expectedValues = e;
                waitHandle.Set();
            };
            waitHandle.WaitOne(1000);

            ProcessCycle();

            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Average)).Should().Be(expectedValues!.Average);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Difference)).Should().Be(expectedValues.Difference);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Maximum)).Should().Be(expectedValues.Maximum);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Minimum)).Should().Be(expectedValues.Minimum);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Sum)).Should().Be(expectedValues.Sum);
        }

        [Fact]
        public void UpdatesOutputCorrectlyWithMultipleInputValues()
        {
            ChangeSettingValue(nameof(IntervalStatistic.Settings.CompressionTime), 200);
            ChangeInputValue(nameof(IntervalStatistic.ProcessDataInputs.Value), 2.0);
            ProcessCycle();
            var fb = GetRuntimeFunctionBlock();
            var weightedAverage = AverageExtractor.GetWeightedAverage(fb);
            using var waitHandle = new ManualResetEvent(false);
            AveragingTimeExpiredEventArgs? expectedValues = null;
            weightedAverage.AveragingTimeExpired += (_, e) =>
            {
                if (expectedValues is not null)
                {
                    expectedValues = e;
                    waitHandle.Set();
                }
                else
                    expectedValues = e;
            };
            Thread.Sleep(300);
            ChangeInputValue(nameof(IntervalStatistic.ProcessDataInputs.Value), 1.0);
            ProcessCycle();
            waitHandle.WaitOne(1000);

            ProcessCycle();

            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Average)).Should().Be(expectedValues!.Average);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Difference)).Should().Be(expectedValues.Difference);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Last)).Should().Be(1.0);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Maximum)).Should().Be(expectedValues.Maximum);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Minimum)).Should().Be(expectedValues.Minimum);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Sum)).Should().Be(expectedValues.Sum);
        }
    }

    public class ProcessCycle_DifferentDefaultValueOnInput() : RuntimeFunctionBlockTests<IntervalStatistic>(GenerateDataflow())
    {
        [Fact]
        public void UpdatesOutputCorrectlyWithNoInputIfDefaultValueDiffersFromDesign()
        {
            ChangeSettingValue(nameof(IntervalStatistic.Settings.CompressionTime), 200);
            ProcessCycle();
            var fb = GetRuntimeFunctionBlock();
            var weightedAverage = AverageExtractor.GetWeightedAverage(fb);
            using var waitHandle = new ManualResetEvent(false);
            AveragingTimeExpiredEventArgs? expectedValues = null;
            weightedAverage.AveragingTimeExpired += (_, e) =>
            {
                expectedValues = e;
                waitHandle.Set();
            };
            waitHandle.WaitOne(1000);

            ProcessCycle();

            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Average)).Should().Be(expectedValues!.Average);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Difference)).Should().Be(expectedValues.Difference);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Last)).Should().Be(double.MaxValue);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Maximum)).Should().Be(expectedValues.Maximum);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Minimum)).Should().Be(expectedValues.Minimum);
            GetOutputValue<double>(nameof(IntervalStatistic.ProcessDataOutputs.Sum)).Should().Be(expectedValues.Sum);
        }

        private static Dataflow ExtractDataflow(DataflowInfo dataflowInfo)
            => dataflowInfo.Dataflow as Dataflow ?? throw new InvalidOperationException("These test cases can only be executed if Core data model is used.");

        private static (DataflowInfo, IEnumerable<Assembly>) GenerateDataflow()
        {
            var assemblies = CreateDefaultAssemblies();
            var info = GenerateDefaultDataflow(assemblies);
            var dataflow = ExtractDataflow(info);
            var block = dataflow.RootContainer.FunctionBlocks.Single();
            block.ProcessDataInputs.Single().Value = double.MaxValue;
            return (info, assemblies.AllAssemblies);
        }
    }

    private static class AverageExtractor
    {
        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_average")]
        public static extern ref WeightedAverage GetWeightedAverage(IntervalStatistic intervalStatistic);
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AwesomeAssertions;
using ViciOne.Core.Dataflow.DataModel;
using ViciOne.Core.Runtime.Execution;
using ViciOne.Math;
using Xunit;

public class RandomTests
{
    public class ProcessCycle_Change : RuntimeFunctionBlockTests<Random>
    {
        [Fact]
        public void CalculatesNewResultsIfInputIsTrue()
        {
            var outputDoubleBefore = GetOutputValue<double>(nameof(Random.ProcessDataOutputs.NextDouble));
            var outputLongBefore = GetOutputValue<long>(nameof(Random.ProcessDataOutputs.NextLong));
            ChangeInputValue(nameof(Random.ProcessDataInputs.Trigger), true);

            ProcessCycle();

            var outputDoubleAfter = GetOutputValue<double>(nameof(Random.ProcessDataOutputs.NextDouble));
            var outputLongAfter = GetOutputValue<long>(nameof(Random.ProcessDataOutputs.NextLong));

            outputDoubleAfter.Should().NotBe(outputDoubleBefore);
            outputLongAfter.Should().NotBe(outputLongBefore);
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(20, 21)]
        [InlineData(1024, 1025)]
        [InlineData(11399, 11400)]
        [InlineData(-12000, -11999)]
        public void CalculatesNewResultsInLimits(int minimum, int maximum)
        {
            ChangeSettingValue(nameof(Random.Settings.Minimum), minimum);
            ChangeSettingValue(nameof(Random.Settings.Maximum), maximum);
            ChangeInputValue(nameof(Random.ProcessDataInputs.Trigger), true);

            ProcessCycle();

            var outputDoubleAfter = GetOutputValue<double>(nameof(Random.ProcessDataOutputs.NextDouble));
            var outputLongAfter = GetOutputValue<long>(nameof(Random.ProcessDataOutputs.NextLong));
            outputDoubleAfter.Should().BeInRange(minimum, maximum);
            outputLongAfter.Should().BeInRange(minimum, maximum);
        }

        [Fact]
        public void DoesNotProcessInputIfInputIsFalse()
        {
            var outputDoubleBefore = GetOutputValue<double>(nameof(Random.ProcessDataOutputs.NextDouble));
            var outputLongBefore = GetOutputValue<long>(nameof(Random.ProcessDataOutputs.NextLong));
            ChangeInputValue(nameof(Random.ProcessDataInputs.Trigger), false);

            ProcessCycle();

            var outputDoubleAfter = GetOutputValue<double>(nameof(Random.ProcessDataOutputs.NextDouble));
            var outputLongAfter = GetOutputValue<long>(nameof(Random.ProcessDataOutputs.NextLong));
            outputDoubleAfter.Should().Be(outputDoubleBefore);
            outputLongAfter.Should().Be(outputLongBefore);
        }

        [Fact]
        public void DoesNotProcessInputIfBlockIsNotEnabled()
        {
            var outputDoubleBefore = GetOutputValue<double>(nameof(Random.ProcessDataOutputs.NextDouble));
            var outputLongBefore = GetOutputValue<long>(nameof(Random.ProcessDataOutputs.NextLong));
            ChangeInputValue(nameof(Random.ProcessDataInputs.Trigger), true);
            ChangeEnabled(false);

            ProcessCycle();

            var outputDoubleAfter = GetOutputValue<double>(nameof(Random.ProcessDataOutputs.NextDouble));
            var outputLongAfter = GetOutputValue<long>(nameof(Random.ProcessDataOutputs.NextLong));
            outputDoubleAfter.Should().Be(outputDoubleBefore);
            outputLongAfter.Should().Be(outputLongBefore);
        }

        [Fact]
        public void DoesNotProcessSettingIfBlockIsNotEnabled()
        {
            ChangeEnabled(false);
            ProcessCycle();
            var outputDoubleBefore = GetOutputValue<double>(nameof(Random.ProcessDataOutputs.NextDouble));
            var outputLongBefore = GetOutputValue<long>(nameof(Random.ProcessDataOutputs.NextLong));
            ChangeSettingValue(nameof(Random.Settings.Maximum), 200);

            ProcessCycle();

            var outputDoubleAfter = GetOutputValue<double>(nameof(Random.ProcessDataOutputs.NextDouble));
            var outputLongAfter = GetOutputValue<long>(nameof(Random.ProcessDataOutputs.NextLong));
            outputDoubleAfter.Should().Be(outputDoubleBefore);
            outputLongAfter.Should().Be(outputLongBefore);
        }
    }

    public class ProcessCycle_Cyclic() : RuntimeFunctionBlockTests<Random>(GenerateDataflow())
    {
        [Fact]
        public void CalculatesNewResultsWithoutInputChange()
        {
            StartEngine();
            var cyclesToRun = 5;
            var outputValuesDouble = new List<double>();
            var outputValuesLong = new List<long>();
            BlockExecutor!.OutputValuesChanged += (_, _) =>
            {
                outputValuesDouble.Add(GetOutputValue<double>(nameof(Random.ProcessDataOutputs.NextDouble)));
                outputValuesLong.Add(GetOutputValue<long>(nameof(Random.ProcessDataOutputs.NextLong)));
            };

            for (var i = 0; i < cyclesToRun; i++)
                ProcessCycle();

            outputValuesDouble.Should()
                .HaveCount(cyclesToRun).And
                .OnlyHaveUniqueItems();
            outputValuesLong.Should()
                .HaveCount(cyclesToRun).And
                .OnlyHaveUniqueItems();
        }

        private static Dataflow ExtractDataflow(DataflowInfo dataflowInfo)
            => dataflowInfo.Dataflow as Dataflow ?? throw new System.InvalidOperationException("These test cases can only be executed if Core data model is used.");

        private static (DataflowInfo, IEnumerable<Assembly>) GenerateDataflow()
        {
            var assemblies = CreateDefaultAssemblies();
            var info = GenerateDefaultDataflow(assemblies);
            var dataflow = ExtractDataflow(info);
            dataflow.RootContainer.FunctionBlocks.First().RunMode = ViciOne.Core.Contracts.DataModel.RunMode.Cyclic;
            return (info, assemblies.AllAssemblies);
        }
    }
}

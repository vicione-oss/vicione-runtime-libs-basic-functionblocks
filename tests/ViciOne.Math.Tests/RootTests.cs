using AwesomeAssertions;
using ViciOne.Math;
using Xunit;

public class RootTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<Root>
    {
        [Theory]
        [InlineData(4.2, 1.0, 4.2)]
        [InlineData(16.0, 2.0, 4.0)]
        [InlineData(8.0, 3.0, 2.0)]
        [InlineData(4.0, -2.0, 0.5)]
        public void CalculatesCorrectResult(double radicand, double index, double expectedValue)
        {
            ChangeInputValue(nameof(Root.ProcessDataInputs.Radicand), radicand);
            ChangeSettingValue(nameof(Root.Settings.Index), index);

            ProcessCycle();

            var outputValue = GetOutputValue<double>(nameof(Root.ProcessDataOutputs.Value));
            outputValue.Should().Be(expectedValue);
        }

        [Fact]
        public void CalculatesCorrectResultIfOnlyInputChanged()
        {
            ProcessCycle();
            var radicand = 16.0;
            ChangeInputValue(nameof(Root.ProcessDataInputs.Radicand), radicand);
            var index = GetSettingValue<double>(nameof(Root.Settings.Index));
            var expectedOutputValue = System.Math.Pow(radicand, 1.0 / index);

            ProcessCycle();

            var outputValue = GetOutputValue<double>(nameof(Root.ProcessDataOutputs.Value));
            outputValue.Should().Be(expectedOutputValue);
        }

        [Fact]
        public void CalculatesCorrectResultIfOnlySettingChanged()
        {
            ProcessCycle();
            var index = 3.0;
            ChangeSettingValue(nameof(Root.Settings.Index), index);
            var radicand = GetInputValue<double>(nameof(Root.ProcessDataInputs.Radicand));
            var expectedOutputValue = System.Math.Pow(radicand, 1.0 / index);

            ProcessCycle();

            var outputValue = GetOutputValue<double>(nameof(Root.ProcessDataOutputs.Value));
            outputValue.Should().Be(expectedOutputValue);
        }

        [Fact]
        public void DoesNotProcessInputIfBlockIsNotEnabled()
        {
            var outputValueBefore = GetOutputValue<double>(nameof(Root.ProcessDataOutputs.Value));
            ChangeInputValue(nameof(Root.ProcessDataInputs.Radicand), 16.0);
            ChangeEnabled(false);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<double>(nameof(Root.ProcessDataOutputs.Value));
            outputValueAfter.Should().Be(outputValueBefore);
        }

        [Fact]
        public void DoesNotProcessSettingIfBlockIsNotEnabled()
        {
            ChangeEnabled(false);
            ProcessCycle();
            var outputValueBefore = GetOutputValue<double>(nameof(Root.ProcessDataOutputs.Value));
            ChangeSettingValue(nameof(Root.Settings.Index), 3.0);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<double>(nameof(Root.ProcessDataOutputs.Value));
            outputValueAfter.Should().Be(outputValueBefore);
        }
    }
}

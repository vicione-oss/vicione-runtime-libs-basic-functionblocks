using AwesomeAssertions;
using ViciOne.Converter;
using Xunit;

public class BooleanToDoubleTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<BooleanToDouble>
    {
        [Fact]
        public void CalculatesCorrectIfBlockWasDisabled()
        {
            ChangeInputValue(nameof(BooleanToDouble.ProcessDataInputs.Value), true);
            ChangeEnabled(false);
            ProcessCycle();
            ChangeEnabled(true);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<double>(nameof(BooleanToDouble.ProcessDataOutputs.Value));
            outputValueAfter.Should().Be(1.0);
        }

        [Theory]
        [InlineData(true, 1.0)]
        [InlineData(false, 0.0)]
        public void CalculatesCorrectResult(bool input, double expectedValue)
        {
            ChangeInputValue(nameof(BooleanToDouble.ProcessDataInputs.Value), input);

            ProcessCycle();

            var outputValue = GetOutputValue<double>(nameof(BooleanToDouble.ProcessDataOutputs.Value));
            outputValue.Should().Be(expectedValue);
        }

        [Fact]
        public void DoesNothingIfBlockIsNotEnabled()
        {
            var outputValueBefore = GetOutputValue<double>(nameof(BooleanToDouble.ProcessDataOutputs.Value));
            ChangeInputValue(nameof(BooleanToDouble.ProcessDataInputs.Value), true);
            ChangeEnabled(false);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<double>(nameof(BooleanToDouble.ProcessDataOutputs.Value));
            outputValueAfter.Should().Be(outputValueBefore);
        }
    }
}

using AwesomeAssertions;
using ViciOne.Converter;
using Xunit;

public class LongToDoubleTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<LongToDouble>
    {
        [Fact]
        public void CalculatesCorrectIfBlockWasDisabled()
        {
            ChangeInputValue(nameof(LongToDouble.ProcessDataInputs.Value), 10L);
            ChangeEnabled(false);
            ProcessCycle();
            ChangeEnabled(true);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<double>(nameof(LongToDouble.ProcessDataOutputs.Value));
            outputValueAfter.Should().Be(10.0);
        }

        [Theory]
        [InlineData(11, 11.0)]
        [InlineData(125, 125.0)]
        public void CalculatesCorrectResult(long input, double expectedValue)
        {
            ChangeInputValue(nameof(LongToDouble.ProcessDataInputs.Value), input);

            ProcessCycle();

            var outputValue = GetOutputValue<double>(nameof(LongToDouble.ProcessDataOutputs.Value));
            outputValue.Should().Be(expectedValue);
        }

        [Fact]
        public void DoesNothingIfBlockIsNotEnabled()
        {
            var outputValueBefore = GetOutputValue<double>(nameof(LongToDouble.ProcessDataOutputs.Value));
            ChangeInputValue(nameof(LongToDouble.ProcessDataInputs.Value), 12L);
            ChangeEnabled(false);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<double>(nameof(LongToDouble.ProcessDataOutputs.Value));
            outputValueAfter.Should().Be(outputValueBefore);
        }
    }
}

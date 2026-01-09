using AwesomeAssertions;
using ViciOne.Converter;
using Xunit;

public class DoubleToBooleanTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<DoubleToBoolean>
    {
        [Fact]
        public void CalculatesCorrectIfBlockWasDisabled()
        {
            ChangeInputValue(nameof(DoubleToBoolean.ProcessDataInputs.Value), 12.4);
            ChangeEnabled(false);
            ProcessCycle();
            ChangeEnabled(true);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<bool>(nameof(DoubleToBoolean.ProcessDataOutputs.Value));
            outputValueAfter.Should().Be(true);
        }

        [Theory]
        [InlineData(1.0, true)]
        [InlineData(10.0, true)]
        [InlineData(-0.5, true)]
        [InlineData(0.0, false)]
        public void CalculatesCorrectResult(double input, bool expectedValue)
        {
            ChangeInputValue(nameof(DoubleToBoolean.ProcessDataInputs.Value), input);

            ProcessCycle();

            var outputValue = GetOutputValue<bool>(nameof(DoubleToBoolean.ProcessDataOutputs.Value));
            outputValue.Should().Be(expectedValue);
        }

        [Fact]
        public void DoesNothingIfBlockIsNotEnabled()
        {
            var outputValueBefore = GetOutputValue<bool>(nameof(DoubleToBoolean.ProcessDataOutputs.Value));
            ChangeInputValue(nameof(DoubleToBoolean.ProcessDataInputs.Value), 12.4);
            ChangeEnabled(false);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<bool>(nameof(DoubleToBoolean.ProcessDataOutputs.Value));
            outputValueAfter.Should().Be(outputValueBefore);
        }
    }
}

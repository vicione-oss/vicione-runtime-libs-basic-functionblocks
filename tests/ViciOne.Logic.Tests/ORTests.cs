using AwesomeAssertions;
using ViciOne.Logic;
using Xunit;

public class ORTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<OR>
    {
        [Fact]
        public void CalculatesCorrectIfBlockWasDisabled()
        {
            ChangeInputValue(nameof(OR.ProcessDataInputs.A), true);
            ChangeInputValue(nameof(OR.ProcessDataInputs.B), false);
            ChangeEnabled(false);
            ProcessCycle();
            ChangeEnabled(true);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<bool>(nameof(OR.ProcessDataOutputs.Y));
            outputValueAfter.Should().Be(true);
        }

        [Theory]
        [InlineData(false, false, false)]
        [InlineData(false, true, true)]
        [InlineData(true, false, true)]
        [InlineData(true, true, true)]
        public void CalculatesCorrectResult(bool inputA, bool inputB, bool expectedValue)
        {
            ChangeInputValue(nameof(OR.ProcessDataInputs.A), inputA);
            ChangeInputValue(nameof(OR.ProcessDataInputs.B), inputB);

            ProcessCycle();

            var outputValue = GetOutputValue<bool>(nameof(OR.ProcessDataOutputs.Y));
            outputValue.Should().Be(expectedValue);
        }

        [Fact]
        public void DoesNotProcessInputIfBlockIsNotEnabled()
        {
            var outputValueBefore = GetOutputValue<bool>(nameof(OR.ProcessDataOutputs.Y));
            ChangeInputValue(nameof(OR.ProcessDataInputs.A), true);
            ChangeInputValue(nameof(OR.ProcessDataInputs.B), false);
            ChangeEnabled(false);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<bool>(nameof(OR.ProcessDataOutputs.Y));
            outputValueAfter.Should().Be(outputValueBefore);
        }
    }
}

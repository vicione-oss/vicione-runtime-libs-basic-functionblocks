using AwesomeAssertions;
using ViciOne.Logic;
using Xunit;

public class XNORTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<XNOR>
    {
        [Fact]
        public void CalculatesCorrectIfBlockWasDisabled()
        {
            ChangeInputValue(nameof(XNOR.ProcessDataInputs.A), true);
            ChangeInputValue(nameof(XNOR.ProcessDataInputs.B), false);
            ChangeEnabled(false);
            ProcessCycle();
            ChangeEnabled(true);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<bool>(nameof(XNOR.ProcessDataOutputs.Y));
            outputValueAfter.Should().Be(false);
        }

        [Theory]
        [InlineData(false, false, true)]
        [InlineData(false, true, false)]
        [InlineData(true, false, false)]
        [InlineData(true, true, true)]
        public void CalculatesCorrectResult(bool inputA, bool inputB, bool expectedValue)
        {
            ChangeInputValue(nameof(XNOR.ProcessDataInputs.A), inputA);
            ChangeInputValue(nameof(XNOR.ProcessDataInputs.B), inputB);

            ProcessCycle();

            var outputValue = GetOutputValue<bool>(nameof(XNOR.ProcessDataOutputs.Y));
            outputValue.Should().Be(expectedValue);
        }

        [Fact]
        public void DoesNotProcessInputIfBlockIsNotEnabled()
        {
            var outputValueBefore = GetOutputValue<bool>(nameof(XNOR.ProcessDataOutputs.Y));
            ChangeInputValue(nameof(XNOR.ProcessDataInputs.A), true);
            ChangeInputValue(nameof(XNOR.ProcessDataInputs.B), false);
            ChangeEnabled(false);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<bool>(nameof(XNOR.ProcessDataOutputs.Y));
            outputValueAfter.Should().Be(outputValueBefore);
        }
    }
}

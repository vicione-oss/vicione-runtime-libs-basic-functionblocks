using AwesomeAssertions;
using ViciOne.Logic;
using Xunit;

public class ANDTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<AND>
    {
        [Fact]
        public void CalculatesCorrectIfBlockWasDisabled()
        {
            ChangeInputValue(nameof(AND.ProcessDataInputs.A), true);
            ChangeInputValue(nameof(AND.ProcessDataInputs.B), true);
            ChangeEnabled(false);
            ProcessCycle();
            ChangeEnabled(true);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<bool>(nameof(AND.ProcessDataOutputs.Y));
            outputValueAfter.Should().Be(true);
        }

        [Theory]
        [InlineData(false, false, false)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(true, true, true)]
        public void CalculatesCorrectResult(bool inputA, bool inputB, bool expectedValue)
        {
            ChangeInputValue(nameof(AND.ProcessDataInputs.A), inputA);
            ChangeInputValue(nameof(AND.ProcessDataInputs.B), inputB);

            ProcessCycle();

            var outputValue = GetOutputValue<bool>(nameof(AND.ProcessDataOutputs.Y));
            outputValue.Should().Be(expectedValue);
        }

        [Fact]
        public void DoesNotProcessInputIfBlockIsNotEnabled()
        {
            var outputValueBefore = GetOutputValue<bool>(nameof(AND.ProcessDataOutputs.Y));
            ChangeInputValue(nameof(AND.ProcessDataInputs.A), true);
            ChangeInputValue(nameof(AND.ProcessDataInputs.B), true);
            ChangeEnabled(false);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<bool>(nameof(AND.ProcessDataOutputs.Y));
            outputValueAfter.Should().Be(outputValueBefore);
        }
    }
}

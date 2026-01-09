using AwesomeAssertions;
using ViciOne.Logic;
using Xunit;

public class NOTTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<NOT>
    {
        [Fact]
        public void CalculatesCorrectIfBlockWasDisabled()
        {
            ChangeInputValue(nameof(NOT.ProcessDataInputs.A), true);
            ChangeEnabled(false);
            ProcessCycle();
            ChangeEnabled(true);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<bool>(nameof(NOT.ProcessDataOutputs.Y));
            outputValueAfter.Should().Be(false);
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        public void CalculatesCorrectResult(bool inputA, bool expectedValue)
        {
            ChangeInputValue(nameof(NOT.ProcessDataInputs.A), inputA);

            ProcessCycle();

            var outputValue = GetOutputValue<bool>(nameof(NOT.ProcessDataOutputs.Y));
            outputValue.Should().Be(expectedValue);
        }

        [Fact]
        public void DoesNotProcessInputIfBlockIsNotEnabled()
        {
            var outputValueBefore = GetOutputValue<bool>(nameof(NOT.ProcessDataOutputs.Y));
            ChangeInputValue(nameof(NOT.ProcessDataInputs.A), true);
            ChangeEnabled(false);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<bool>(nameof(NOT.ProcessDataOutputs.Y));
            outputValueAfter.Should().Be(outputValueBefore);
        }
    }
}

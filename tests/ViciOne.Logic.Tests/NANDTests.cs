using AwesomeAssertions;
using ViciOne.Logic;
using Xunit;

public class NANDTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<NAND>
    {
        [Fact]
        public void CalculatesCorrectIfBlockWasDisabled()
        {
            ChangeInputValue(nameof(NAND.ProcessDataInputs.A), true);
            ChangeInputValue(nameof(NAND.ProcessDataInputs.B), true);
            ChangeEnabled(false);
            ProcessCycle();
            ChangeEnabled(true);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<bool>(nameof(NAND.ProcessDataOutputs.Y));
            outputValueAfter.Should().Be(false);
        }

        [Theory]
        [InlineData(false, false, true)]
        [InlineData(false, true, true)]
        [InlineData(true, false, true)]
        [InlineData(true, true, false)]
        public void CalculatesCorrectResult(bool inputA, bool inputB, bool expectedValue)
        {
            ChangeInputValue(nameof(NAND.ProcessDataInputs.A), inputA);
            ChangeInputValue(nameof(NAND.ProcessDataInputs.B), inputB);

            ProcessCycle();

            var outputValue = GetOutputValue<bool>(nameof(NAND.ProcessDataOutputs.Y));
            outputValue.Should().Be(expectedValue);
        }

        [Fact]
        public void DoesNotProcessInputIfBlockIsNotEnabled()
        {
            var outputValueBefore = GetOutputValue<bool>(nameof(NAND.ProcessDataOutputs.Y));
            ChangeInputValue(nameof(NAND.ProcessDataInputs.A), true);
            ChangeInputValue(nameof(NAND.ProcessDataInputs.B), true);
            ChangeEnabled(false);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<bool>(nameof(NAND.ProcessDataOutputs.Y));
            outputValueAfter.Should().Be(outputValueBefore);
        }
    }
}

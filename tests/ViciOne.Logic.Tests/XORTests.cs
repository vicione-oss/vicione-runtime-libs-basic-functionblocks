using AwesomeAssertions;
using ViciOne.Logic;
using Xunit;

public class XORTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<XOR>
    {
        [Fact]
        public void CalculatesCorrectIfBlockWasDisabled()
        {
            ChangeInputValue(nameof(XOR.ProcessDataInputs.A), true);
            ChangeInputValue(nameof(XOR.ProcessDataInputs.B), false);
            ChangeEnabled(false);
            ProcessCycle();
            ChangeEnabled(true);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<bool>(nameof(XOR.ProcessDataOutputs.Y));
            outputValueAfter.Should().Be(true);
        }

        [Theory]
        [InlineData(false, false, false)]
        [InlineData(false, true, true)]
        [InlineData(true, false, true)]
        [InlineData(true, true, false)]
        public void CalculatesCorrectResult(bool inputA, bool inputB, bool expectedValue)
        {
            ChangeInputValue(nameof(XOR.ProcessDataInputs.A), inputA);
            ChangeInputValue(nameof(XOR.ProcessDataInputs.B), inputB);

            ProcessCycle();

            var outputValue = GetOutputValue<bool>(nameof(XOR.ProcessDataOutputs.Y));
            outputValue.Should().Be(expectedValue);
        }

        [Fact]
        public void DoesNotProcessInputIfBlockIsNotEnabled()
        {
            var outputValueBefore = GetOutputValue<bool>(nameof(XOR.ProcessDataOutputs.Y));
            ChangeInputValue(nameof(XOR.ProcessDataInputs.A), true);
            ChangeInputValue(nameof(XOR.ProcessDataInputs.B), false);
            ChangeEnabled(false);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<bool>(nameof(XOR.ProcessDataOutputs.Y));
            outputValueAfter.Should().Be(outputValueBefore);
        }
    }
}

using AwesomeAssertions;
using ViciOne.Logic;
using Xunit;

public class NORTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<NOR>
    {
        [Fact]
        public void CalculatesCorrectIfBlockWasDisabled()
        {
            ChangeInputValue(nameof(NOR.ProcessDataInputs.A), false);
            ChangeInputValue(nameof(NOR.ProcessDataInputs.B), false);
            ChangeEnabled(false);
            ProcessCycle();
            ChangeEnabled(true);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<bool>(nameof(NOR.ProcessDataOutputs.Y));
            outputValueAfter.Should().Be(true);
        }

        [Theory]
        [InlineData(false, false, true)]
        [InlineData(false, true, false)]
        [InlineData(true, false, false)]
        [InlineData(true, true, false)]
        public void CalculatesCorrectResult(bool inputA, bool inputB, bool expectedValue)
        {
            ChangeInputValue(nameof(NOR.ProcessDataInputs.A), inputA);
            ChangeInputValue(nameof(NOR.ProcessDataInputs.B), inputB);

            ProcessCycle();

            var outputValue = GetOutputValue<bool>(nameof(NOR.ProcessDataOutputs.Y));
            outputValue.Should().Be(expectedValue);
        }

        [Fact]
        public void DoesNotProcessInputIfBlockIsNotEnabled()
        {
            var outputValueBefore = GetOutputValue<bool>(nameof(NOR.ProcessDataOutputs.Y));
            ChangeInputValue(nameof(NOR.ProcessDataInputs.A), true);
            ChangeInputValue(nameof(NOR.ProcessDataInputs.B), true);
            ChangeEnabled(false);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<bool>(nameof(NOR.ProcessDataOutputs.Y));
            outputValueAfter.Should().Be(outputValueBefore);
        }
    }
}

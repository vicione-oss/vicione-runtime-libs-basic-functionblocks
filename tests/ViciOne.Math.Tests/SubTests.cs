using AwesomeAssertions;
using ViciOne.Math;
using Xunit;

public class SubTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<Sub>
    {
        [Fact]
        public void CalculatesCorrectResult()
        {
            ChangeInputValue(nameof(Sub.ProcessDataInputs.Minuend), 42.0);
            ChangeInputValue(nameof(Sub.ProcessDataInputs.Subtrahend), 10.0);

            ProcessCycle();

            var difference = GetOutputValue<double>(nameof(Sub.ProcessDataOutputs.Difference));
            difference.Should().Be(32);
        }

        [Fact]
        public void CalculatesCorrectResultIfBlockWasDisabled()
        {
            ChangeInputValue(nameof(Sub.ProcessDataInputs.Minuend), 42.0);
            ChangeInputValue(nameof(Sub.ProcessDataInputs.Subtrahend), 10.0);
            ChangeEnabled(false);
            ProcessCycle();
            ChangeEnabled(true);

            ProcessCycle();

            var difference = GetOutputValue<double>(nameof(Sub.ProcessDataOutputs.Difference));
            difference.Should().Be(32);
        }

        [Fact]
        public void DoesNothingIfBlockIsNotEnabled()
        {
            var differenceBefore = GetOutputValue<double>(nameof(Sub.ProcessDataOutputs.Difference));
            ChangeInputValue(nameof(Sub.ProcessDataInputs.Minuend), 42.0);
            ChangeInputValue(nameof(Sub.ProcessDataInputs.Subtrahend), 10.0);
            ChangeEnabled(false);

            ProcessCycle();

            var difference = GetOutputValue<double>(nameof(Sub.ProcessDataOutputs.Difference));
            difference.Should().Be(differenceBefore);
        }
    }
}

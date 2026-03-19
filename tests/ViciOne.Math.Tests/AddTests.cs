using AwesomeAssertions;
using ViciOne.Math;
using Xunit;

public class AddTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<Add>
    {
        [Fact]
        public void CalculatesCorrectResult()
        {
            ChangeInputValue(nameof(Add.ProcessDataInputs.Addend1), 42.0);
            ChangeInputValue(nameof(Add.ProcessDataInputs.Addend2), 10.0);

            ProcessCycle();

            var sum = GetOutputValue<double>(nameof(Add.ProcessDataOutputs.Sum));
            sum.Should().Be(52);
        }

        [Fact]
        public void CalculatesCorrectResultIfBlockWasDisabled()
        {
            ChangeInputValue(nameof(Add.ProcessDataInputs.Addend1), 42.0);
            ChangeInputValue(nameof(Add.ProcessDataInputs.Addend2), 10.0);
            ChangeEnabled(false);
            ProcessCycle();
            ChangeEnabled(true);

            ProcessCycle();

            var sum = GetOutputValue<double>(nameof(Add.ProcessDataOutputs.Sum));
            sum.Should().Be(52);
        }

        [Fact]
        public void DoesNothingIfBlockIsNotEnabled()
        {
            var sumBefore = GetOutputValue<double>(nameof(Add.ProcessDataOutputs.Sum));
            ChangeInputValue(nameof(Add.ProcessDataInputs.Addend1), 42.0);
            ChangeInputValue(nameof(Add.ProcessDataInputs.Addend2), 10.0);
            ChangeEnabled(false);

            ProcessCycle();

            var sum = GetOutputValue<double>(nameof(Add.ProcessDataOutputs.Sum));
            sum.Should().Be(sumBefore);
        }
    }
}

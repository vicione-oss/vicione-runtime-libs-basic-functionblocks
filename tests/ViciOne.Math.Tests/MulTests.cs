using AwesomeAssertions;
using ViciOne.Math;
using Xunit;

public class MulTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<Mul>
    {
        [Fact]
        public void CalculatesCorrectResult()
        {
            ChangeInputValue(nameof(Mul.ProcessDataInputs.Factor1), 8.0);
            ChangeInputValue(nameof(Mul.ProcessDataInputs.Factor2), 0.5);

            ProcessCycle();

            var product = GetOutputValue<double>(nameof(Mul.ProcessDataOutputs.Product));
            product.Should().Be(4.0);
        }

        [Fact]
        public void DoesNothingIfBlockIsNotEnabled()
        {
            var productBefore = GetOutputValue<double>(nameof(Mul.ProcessDataOutputs.Product));
            ChangeInputValue(nameof(Mul.ProcessDataInputs.Factor1), 8.0);
            ChangeInputValue(nameof(Mul.ProcessDataInputs.Factor2), 0.5);
            ChangeEnabled(false);

            ProcessCycle();

            var product = GetOutputValue<double>(nameof(Mul.ProcessDataOutputs.Product));
            product.Should().Be(productBefore);
        }
    }
}

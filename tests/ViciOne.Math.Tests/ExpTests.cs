using AwesomeAssertions;
using ViciOne.Math;
using Xunit;

public class ExpTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<Exp>
    {
        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 2.718281828459045)]
        [InlineData(-1, 0.36787944117144233)]
        public void CalculatesCorrectResult(double x, double result)
        {
            ChangeInputValue(nameof(Exp.ProcessDataInputs.X), x);

            ProcessCycle();

            var exp = GetOutputValue<double>(nameof(Exp.ProcessDataOutputs.Exp));
            exp.Should().Be(result);
        }

        [Fact]
        public void DoesNothingIfBlockIsNotEnabled()
        {
            var expBefore = GetOutputValue<double>(nameof(Exp.ProcessDataOutputs.Exp));
            ChangeInputValue(nameof(Exp.ProcessDataInputs.X), 5.0);
            ChangeEnabled(false);

            ProcessCycle();

            var exp = GetOutputValue<double>(nameof(Exp.ProcessDataOutputs.Exp));
            exp.Should().Be(expBefore);
        }
    }
}

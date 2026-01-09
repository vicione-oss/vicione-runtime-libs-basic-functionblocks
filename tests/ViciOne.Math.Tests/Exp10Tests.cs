using AwesomeAssertions;
using ViciOne.Math;
using Xunit;

public class Exp10Tests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<Exp10>
    {
        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 10)]
        [InlineData(3, 1000)]
        [InlineData(-1, 0.1)]
        [InlineData(-3, 0.001)]
        public void CalculatesCorrectResult(double x, double result)
        {
            ChangeInputValue(nameof(Exp10.ProcessDataInputs.X), x);

            ProcessCycle();

            var exp10 = GetOutputValue<double>(nameof(Exp10.ProcessDataOutputs.Exp10));
            exp10.Should().Be(result);
        }

        [Fact]
        public void DoesNothingIfBlockIsNotEnabled()
        {
            var exp10Before = GetOutputValue<double>(nameof(Exp10.ProcessDataOutputs.Exp10));
            ChangeInputValue(nameof(Exp10.ProcessDataInputs.X), 5.0);
            ChangeEnabled(false);

            ProcessCycle();

            var exp10 = GetOutputValue<double>(nameof(Exp10.ProcessDataOutputs.Exp10));
            exp10.Should().Be(exp10Before);
        }
    }
}

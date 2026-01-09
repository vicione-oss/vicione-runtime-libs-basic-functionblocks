using AwesomeAssertions;
using ViciOne.Math;
using Xunit;

public class PowerTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<Power>
    {
        [Theory]
        [InlineData(2, 0, 1)]
        [InlineData(2, 1, 2)]
        [InlineData(2, 2, 4)]
        [InlineData(2, 10, 1024)]
        [InlineData(2, 16, 65536)]
        [InlineData(2, -2, 0.25)]
        [InlineData(-2, 2, 4)]
        public void CalculatesCorrectResult(double x, double y, double result)
        {
            ChangeInputValue(nameof(Power.ProcessDataInputs.X), x);
            ChangeInputValue(nameof(Power.ProcessDataInputs.Y), y);

            ProcessCycle();

            var power = GetOutputValue<double>(nameof(Power.ProcessDataOutputs.Power));
            power.Should().Be(result);
        }

        [Fact]
        public void DoesNothingIfBlockIsNotEnabled()
        {
            var powerBefore = GetOutputValue<double>(nameof(Power.ProcessDataOutputs.Power));
            ChangeInputValue(nameof(Power.ProcessDataInputs.X), 5.0);
            ChangeInputValue(nameof(Power.ProcessDataInputs.Y), 2.0);
            ChangeEnabled(false);

            ProcessCycle();

            var power = GetOutputValue<double>(nameof(Power.ProcessDataOutputs.Power));
            power.Should().Be(powerBefore);
        }
    }
}

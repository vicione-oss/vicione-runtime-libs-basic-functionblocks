using AwesomeAssertions;
using ViciOne.Math;
using Xunit;

public class Log10Tests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<Log10>
    {
        [Theory]
        [InlineData(1, 0)]
        [InlineData(0.5, -0.3010299956639812)]
        [InlineData(4, 0.6020599913279624)]
        public void CalculatesCorrectResult(double x, double result)
        {
            ChangeInputValue(nameof(Log10.ProcessDataInputs.X), x);

            ProcessCycle();

            var log10 = GetOutputValue<double>(nameof(Log10.ProcessDataOutputs.Log10));
            log10.Should().Be(result);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void CalculateOnlyIfXGreatherThanZero(double invalidValue)
        {
            ChangeInputValue(nameof(Log10.ProcessDataInputs.X), 4.0);
            ProcessCycle();
            var log10Before = GetOutputValue<double>(nameof(Log10.ProcessDataOutputs.Log10));
            ChangeInputValue(nameof(Log10.ProcessDataInputs.X), invalidValue);

            ProcessCycle();

            var log10 = GetOutputValue<double>(nameof(Log10.ProcessDataOutputs.Log10));
            log10.Should().Be(log10Before);
        }

        [Fact]
        public void DoesNothingIfBlockIsNotEnabled()
        {
            var log10Before = GetOutputValue<double>(nameof(Log10.ProcessDataOutputs.Log10));
            ChangeInputValue(nameof(Log10.ProcessDataInputs.X), 5.0);
            ChangeEnabled(false);

            ProcessCycle();

            var log10 = GetOutputValue<double>(nameof(Log10.ProcessDataOutputs.Log10));
            log10.Should().Be(log10Before);
        }
    }
}

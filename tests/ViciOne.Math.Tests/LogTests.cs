using AwesomeAssertions;
using ViciOne.Math;
using Xunit;

public class LogTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<Log>
    {
        [Theory]
        [InlineData(1, 0)]
        [InlineData(10, 2.302585092994046)]
        public void CalculatesCorrectResult(double x, double result)
        {
            ChangeInputValue(nameof(Log.ProcessDataInputs.X), x);

            ProcessCycle();

            var log = GetOutputValue<double>(nameof(Log.ProcessDataOutputs.Log));
            log.Should().Be(result);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void CalculateOnlyIfXGreatherThanZero(double invalidValue)
        {
            ChangeInputValue(nameof(Log.ProcessDataInputs.X), 4.0);
            ProcessCycle();
            var logBefore = GetOutputValue<double>(nameof(Log.ProcessDataOutputs.Log));
            ChangeInputValue(nameof(Log.ProcessDataInputs.X), invalidValue);

            ProcessCycle();

            var log = GetOutputValue<double>(nameof(Log.ProcessDataOutputs.Log));
            log.Should().Be(logBefore);
        }

        [Fact]
        public void DoesNothingIfBlockIsNotEnabled()
        {
            var logBefore = GetOutputValue<double>(nameof(Log.ProcessDataOutputs.Log));
            ChangeInputValue(nameof(Log.ProcessDataInputs.X), 5.0);
            ChangeEnabled(false);

            ProcessCycle();

            var log = GetOutputValue<double>(nameof(Log.ProcessDataOutputs.Log));
            log.Should().Be(logBefore);
        }
    }
}

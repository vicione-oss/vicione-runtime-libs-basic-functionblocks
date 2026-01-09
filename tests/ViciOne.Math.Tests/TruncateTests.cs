using AwesomeAssertions;
using ViciOne.Math;
using Xunit;

public class TruncateTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<Truncate>
    {
        [Theory]
        [InlineData(4.2, 4.0)]
        [InlineData(-4.2, -4.0)]
        public void CalculatesCorrectResult(double input, double expectedResult)
        {
            ChangeInputValue(nameof(Truncate.ProcessDataInputs.Value), input);

            ProcessCycle();

            var result = GetOutputValue<double>(nameof(Truncate.ProcessDataOutputs.Truncate));
            result.Should().Be(expectedResult);
        }

        [Fact]
        public void DoesNothingIfBlockIsNotEnabled()
        {
            var outputBefore = GetOutputValue<double>(nameof(Truncate.ProcessDataOutputs.Truncate));
            ChangeInputValue(nameof(Truncate.ProcessDataInputs.Value), 4.2);
            ChangeEnabled(false);

            ProcessCycle();

            var output = GetOutputValue<double>(nameof(Truncate.ProcessDataOutputs.Truncate));
            output.Should().Be(outputBefore);
        }
    }
}

using AwesomeAssertions;
using ViciOne.Math;
using Xunit;

public class FloorTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<Floor>
    {
        [Theory]
        [InlineData(4.2, 4.0)]
        [InlineData(-4.2, -5.0)]
        public void CalculatesCorrectResult(double input, double expectedResult)
        {
            ChangeInputValue(nameof(Floor.ProcessDataInputs.Value), input);

            ProcessCycle();

            var result = GetOutputValue<double>(nameof(Floor.ProcessDataOutputs.Floor));
            result.Should().Be(expectedResult);
        }

        [Fact]
        public void DoesNothingIfBlockIsNotEnabled()
        {
            var outputBefore = GetOutputValue<double>(nameof(Floor.ProcessDataOutputs.Floor));
            ChangeInputValue(nameof(Floor.ProcessDataInputs.Value), 4.2);
            ChangeEnabled(false);

            ProcessCycle();

            var output = GetOutputValue<double>(nameof(Floor.ProcessDataOutputs.Floor));
            output.Should().Be(outputBefore);
        }
    }
}

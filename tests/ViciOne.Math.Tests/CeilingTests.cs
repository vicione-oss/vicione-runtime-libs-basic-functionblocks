using AwesomeAssertions;
using ViciOne.Math;
using Xunit;

public class CeilingTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<Ceiling>
    {
        [Theory]
        [InlineData(4.2, 5.0)]
        [InlineData(-4.2, -4.0)]
        public void CalculatesCorrectResult(double input, double expectedResult)
        {
            ChangeInputValue(nameof(Ceiling.ProcessDataInputs.Value), input);

            ProcessCycle();

            var result = GetOutputValue<double>(nameof(Ceiling.ProcessDataOutputs.Ceiling));
            result.Should().Be(expectedResult);
        }

        [Fact]
        public void DoesNothingIfBlockIsNotEnabled()
        {
            var outputBefore = GetOutputValue<double>(nameof(Ceiling.ProcessDataOutputs.Ceiling));
            ChangeInputValue(nameof(Ceiling.ProcessDataInputs.Value), 4.2);
            ChangeEnabled(false);

            ProcessCycle();

            var output = GetOutputValue<double>(nameof(Ceiling.ProcessDataOutputs.Ceiling));
            output.Should().Be(outputBefore);
        }
    }
}

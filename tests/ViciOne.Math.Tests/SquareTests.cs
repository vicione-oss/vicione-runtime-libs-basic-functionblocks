using AwesomeAssertions;
using ViciOne.Math;
using Xunit;

public class SquareTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<Square>
    {
        [Theory]
        [InlineData(2, 4)]
        [InlineData(10, 100)]
        [InlineData(11.5, 132.25)]
        [InlineData(-9, 81)]
        [InlineData(0.5, 0.25)]
        [InlineData(-0.3, 0.09)]
        public void CalculatesCorrectResult(double x, double result)
        {
            ChangeInputValue(nameof(Square.ProcessDataInputs.X), x);

            ProcessCycle();

            var power = GetOutputValue<double>(nameof(Square.ProcessDataOutputs.Square));
            power.Should().Be(result);
        }

        [Fact]
        public void DoesNothingIfBlockIsNotEnabled()
        {
            var squareBefore = GetOutputValue<double>(nameof(Square.ProcessDataOutputs.Square));
            ChangeInputValue(nameof(Square.ProcessDataInputs.X), 5.0);
            ChangeEnabled(false);

            ProcessCycle();

            var square = GetOutputValue<double>(nameof(Square.ProcessDataOutputs.Square));
            square.Should().Be(squareBefore);
        }
    }
}

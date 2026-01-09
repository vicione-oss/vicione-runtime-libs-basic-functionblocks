using AwesomeAssertions;
using ViciOne.Math;
using Xunit;

public class AbsDiffTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<AbsDiff>
    {
        [Theory]
        [InlineData(10, -10, 20)]
        [InlineData(-10, 10, 20)]
        [InlineData(10, 15, 5)]
        [InlineData(-1, -3, 2)]
        public void CalculatesCorrectResult(double ax, double ay, double expectedDiff)
        {
            ChangeInputValue(nameof(AbsDiff.ProcessDataInputs.Ax), ax);
            ChangeInputValue(nameof(AbsDiff.ProcessDataInputs.Ay), ay);

            ProcessCycle();

            var diff = GetOutputValue<double>(nameof(AbsDiff.ProcessDataOutputs.Diff));
            diff.Should().Be(expectedDiff);
        }

        [Fact]
        public void DoesNothingIfBlockIsNotEnabled()
        {
            var diffBefore = GetOutputValue<double>(nameof(AbsDiff.ProcessDataOutputs.Diff));
            ChangeInputValue(nameof(AbsDiff.ProcessDataInputs.Ax), 10.0);
            ChangeInputValue(nameof(AbsDiff.ProcessDataInputs.Ay), 20.0);
            ChangeEnabled(false);

            ProcessCycle();

            var diff = GetOutputValue<double>(nameof(AbsDiff.ProcessDataOutputs.Diff));
            diff.Should().Be(diffBefore);
        }
    }
}

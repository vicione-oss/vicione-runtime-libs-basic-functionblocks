using AwesomeAssertions;
using ViciOne.Math;
using Xunit;

public class EqualityTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<Equality>
    {
        [Theory]
        [InlineData(1.0, 1.0, true, false, false, false)]
        [InlineData(1.0, 2.0, false, true, false, true)]
        [InlineData(2.0, 1.0, false, true, true, false)]
        public void CalculatesCorrectResultForDoubleInputs(double firstInput, double secondInput, bool expectedEqual, bool expectedUnequal, bool expectedGreaterThan, bool expectedLessThan)
        {
            ChangeInputValue(nameof(Equality.ProcessDataInputs.Double0), firstInput);
            ChangeInputValue(nameof(Equality.ProcessDataInputs.Double1), secondInput);

            ProcessCycle();

            (var equalOutputValue, var unequalOutputValue, var greaterThanOutputValue, var lessThanOutputValue) = GetOutputValues();
            equalOutputValue.Should().Be(expectedEqual);
            unequalOutputValue.Should().Be(expectedUnequal);
            greaterThanOutputValue.Should().Be(expectedGreaterThan);
            lessThanOutputValue.Should().Be(expectedLessThan);
        }

        [Theory]
        [InlineData(1, 1, true, false, false, false)]
        [InlineData(1, 2, false, true, false, true)]
        [InlineData(2, 1, false, true, true, false)]
        public void CalculatesCorrectResultForLongInputs(long firstInput, long secondInput, bool expectedEqual, bool expectedUnequal, bool expectedGreaterThan, bool expectedLessThan)
        {
            // TODO: Remove the first ProcessCycle call after upgrade to Core v1.0.0.
            //       Currently, all inputs are changed in the first ProcessCycle call, so the block uses double inputs for the comparison.
            //       Maybe it would be better to take the "Connected" flag into account in the block to determine which inputs to use.
            ProcessCycle();
            ChangeInputValue(nameof(Equality.ProcessDataInputs.Long0), firstInput);
            ChangeInputValue(nameof(Equality.ProcessDataInputs.Long1), secondInput);

            ProcessCycle();

            (var equalOutputValue, var unequalOutputValue, var greaterThanOutputValue, var lessThanOutputValue) = GetOutputValues();
            equalOutputValue.Should().Be(expectedEqual);
            unequalOutputValue.Should().Be(expectedUnequal);
            greaterThanOutputValue.Should().Be(expectedGreaterThan);
            lessThanOutputValue.Should().Be(expectedLessThan);
        }

        [Fact]
        public void SetsAllOutputsToFalseIfBlockIsNotEnabled()
        {
            ChangeInputValue(nameof(Equality.ProcessDataInputs.Double0), 1.0);
            ChangeInputValue(nameof(Equality.ProcessDataInputs.Double1), 2.0);
            ChangeEnabled(false);

            ProcessCycle();

            (var equalOutputValue, var unequalOutputValue, var greaterThanOutputValue, var lessThanOutputValue) = GetOutputValues();
            equalOutputValue.Should().BeFalse();
            unequalOutputValue.Should().BeFalse();
            greaterThanOutputValue.Should().BeFalse();
            lessThanOutputValue.Should().BeFalse();
        }

        [Fact]
        public void UsesDoubleInputsIfAllInputsChanged()
        {
            ChangeInputValue(nameof(Equality.ProcessDataInputs.Double0), 1.0);
            ChangeInputValue(nameof(Equality.ProcessDataInputs.Double1), 2.0);
            ChangeInputValue(nameof(Equality.ProcessDataInputs.Long0), 2L);
            ChangeInputValue(nameof(Equality.ProcessDataInputs.Long1), 1L);

            ProcessCycle();

            (var equalOutputValue, var unequalOutputValue, var greaterThanOutputValue, var lessThanOutputValue) = GetOutputValues();
            equalOutputValue.Should().BeFalse();
            unequalOutputValue.Should().BeTrue();
            greaterThanOutputValue.Should().BeFalse();
            lessThanOutputValue.Should().BeTrue();
        }

        private (bool Equal, bool Unequal, bool GreaterThan, bool LessThan) GetOutputValues()
            => (
                GetOutputValue<bool>(nameof(Equality.ProcessDataOutputs.Equal)),
                GetOutputValue<bool>(nameof(Equality.ProcessDataOutputs.Unequal)),
                GetOutputValue<bool>(nameof(Equality.ProcessDataOutputs.GreaterThan)),
                GetOutputValue<bool>(nameof(Equality.ProcessDataOutputs.LessThan)));
    }
}

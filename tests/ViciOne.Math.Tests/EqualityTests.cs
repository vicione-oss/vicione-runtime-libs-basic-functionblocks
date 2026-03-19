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
            // As this block uses the double inputs if all or no inputs are changed, we need to ensure that the engine is running
            // before changing the long inputs so they are marked as changed.
            StartEngine();
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
            ProcessCycle();
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

        [Fact]
        public void UsesDoubleInputsIfNoInputsChanged()
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

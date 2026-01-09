using AwesomeAssertions;
using ViciOne.Math;
using Xunit;

public class RoundTests
{
    public class OnSettingChanged : RuntimeFunctionBlockTests<Round>
    {
        [Fact]
        public void CalculatesCorrectResultIfDecimalPlacesChanged()
        {
            ChangeInputValue(nameof(Round.ProcessDataInputs.Value), 6.66);
            ChangeSettingValue(nameof(Round.Settings.RoundingBehavior), RoundingBehavior.Commercial);
            ProcessCycle();
            ChangeSettingValue(nameof(Round.Settings.DecimalPlaces), 1);
            var expectedOutputValue = 6.7;

            ProcessCycle();

            var outputValue = GetOutputValue<double>(nameof(Round.ProcessDataOutputs.Value));
            outputValue.Should().Be(expectedOutputValue);
        }

        [Fact]
        public void CalculatesCorrectResultIfRoundingBehaviorChanged()
        {
            ChangeInputValue(nameof(Round.ProcessDataInputs.Value), 6.66);
            ChangeSettingValue(nameof(Round.Settings.DecimalPlaces), 1);
            ProcessCycle();
            ChangeSettingValue(nameof(Round.Settings.RoundingBehavior), RoundingBehavior.Down);
            var expectedOutputValue = 6.6;

            ProcessCycle();

            var outputValue = GetOutputValue<double>(nameof(Round.ProcessDataOutputs.Value));
            outputValue.Should().Be(expectedOutputValue);
        }

        [Fact]
        public void DoesNotProcessDecimalPlacesIfBlockIsNotEnabled()
        {
            ChangeEnabled(false);
            ProcessCycle();
            var outputValueBefore = GetOutputValue<double>(nameof(Round.ProcessDataOutputs.Value));
            ChangeSettingValue(nameof(Round.Settings.DecimalPlaces), 1);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<double>(nameof(Round.ProcessDataOutputs.Value));
            outputValueAfter.Should().Be(outputValueBefore);
        }

        [Fact]
        public void DoesNotProcessRoundingBehaviorIfBlockIsNotEnabled()
        {
            ChangeEnabled(false);
            ProcessCycle();
            var outputValueBefore = GetOutputValue<double>(nameof(Round.ProcessDataOutputs.Value));
            ChangeSettingValue(nameof(Round.Settings.RoundingBehavior), RoundingBehavior.Down);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<double>(nameof(Round.ProcessDataOutputs.Value));
            outputValueAfter.Should().Be(outputValueBefore);
        }
    }

    public class ProcessCycle : RuntimeFunctionBlockTests<Round>
    {
        [Fact]
        public void CalculatesCorrectResultIfInputChanged()
        {
            ChangeSettingValue(nameof(Round.Settings.DecimalPlaces), 1);
            ChangeSettingValue(nameof(Round.Settings.RoundingBehavior), RoundingBehavior.Commercial);
            ProcessCycle();
            ChangeInputValue(nameof(Round.ProcessDataInputs.Value), 6.66);
            var expectedOutputValue = 6.7;

            ProcessCycle();

            var outputValue = GetOutputValue<double>(nameof(Round.ProcessDataOutputs.Value));
            outputValue.Should().Be(expectedOutputValue);
        }

        [Fact]
        public void DoesNotProcessInputIfBlockIsNotEnabled()
        {
            var outputValueBefore = GetOutputValue<double>(nameof(Round.ProcessDataOutputs.Value));
            ChangeInputValue(nameof(Round.ProcessDataInputs.Value), 6.66);
            ChangeEnabled(false);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<double>(nameof(Round.ProcessDataOutputs.Value));
            outputValueAfter.Should().Be(outputValueBefore);
        }
    }

    public class RoundValue
    {
        [Theory]
        [InlineData(6.66, 0, RoundingBehavior.Commercial, 7.0)]
        [InlineData(6.66, 0, RoundingBehavior.Up, 7.0)]
        [InlineData(6.66, 0, RoundingBehavior.Down, 6.0)]
        [InlineData(6.66, 1, RoundingBehavior.Commercial, 6.7)]
        [InlineData(6.66, 1, RoundingBehavior.Up, 6.7)]
        [InlineData(6.66, 1, RoundingBehavior.Down, 6.6)]
        [InlineData(6.66, 2, RoundingBehavior.Commercial, 6.66)]
        [InlineData(6.66, 2, RoundingBehavior.Up, 6.66)]
        [InlineData(6.66, 2, RoundingBehavior.Down, 6.66)]
        [InlineData(4.22, 0, RoundingBehavior.Commercial, 4.0)]
        [InlineData(4.22, 0, RoundingBehavior.Up, 5.0)]
        [InlineData(4.22, 0, RoundingBehavior.Down, 4.0)]
        [InlineData(4.22, 1, RoundingBehavior.Commercial, 4.2)]
        [InlineData(4.22, 1, RoundingBehavior.Up, 4.3)]
        [InlineData(4.22, 1, RoundingBehavior.Down, 4.2)]
        [InlineData(4.22, 2, RoundingBehavior.Commercial, 4.22)]
        [InlineData(4.22, 2, RoundingBehavior.Up, 4.22)]
        [InlineData(4.22, 2, RoundingBehavior.Down, 4.22)]
        public void CalculatesCorrectResult(double inputValue, int decimalPlaces, RoundingBehavior roundingBehavior, double expectedResult)
        {
            var result = Round.RoundValue(inputValue, decimalPlaces, roundingBehavior);

            result.Should().Be(expectedResult);
        }
    }
}

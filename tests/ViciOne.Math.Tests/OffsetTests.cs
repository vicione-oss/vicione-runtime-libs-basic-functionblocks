using AwesomeAssertions;
using ViciOne.Math;
using Xunit;

public class OffsetTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<Offset>
    {
        [Theory]
        [InlineData(4.5, 0.0, 4.5)]
        [InlineData(-14.5, 0.0, -14.5)]
        [InlineData(11.5, 10.0, 21.5)]
        [InlineData(-5.5, -10.0, -15.5)]
        public void CalculatesCorrectResult(double input, double offset, double expectedValue)
        {
            ChangeInputValue(nameof(Offset.ProcessDataInputs.Value), input);
            ChangeSettingValue(nameof(Offset.Settings.Offset), offset);

            ProcessCycle();

            var outputValue = GetOutputValue<double>(nameof(Offset.ProcessDataOutputs.Value));
            outputValue.Should().Be(expectedValue);
        }

        [Fact]
        public void DoesNotProcessInputIfBlockIsNotEnabled()
        {
            var outputValueBefore = GetOutputValue<double>(nameof(Offset.ProcessDataOutputs.Value));
            ChangeInputValue(nameof(Offset.ProcessDataInputs.Value), 12.0);
            ChangeEnabled(false);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<double>(nameof(Offset.ProcessDataOutputs.Value));
            outputValueAfter.Should().Be(outputValueBefore);
        }

        [Fact]
        public void DoesNotProcessSettingIfBlockIsNotEnabled()
        {
            ChangeEnabled(false);
            ProcessCycle();
            var outputValueBefore = GetOutputValue<double>(nameof(Offset.ProcessDataOutputs.Value));
            ChangeSettingValue(nameof(Offset.Settings.Offset), 12.0);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<double>(nameof(Offset.ProcessDataOutputs.Value));
            outputValueAfter.Should().Be(outputValueBefore);
        }

        [Fact]
        public void SetsOutputIfInputChanged()
        {
            ChangeInputValue(nameof(Offset.ProcessDataInputs.Value), 12.0);

            ProcessCycle();

            var outputValue = GetOutputValue<double>(nameof(Offset.ProcessDataOutputs.Value));
            outputValue.Should().Be(12.0);
        }

        [Fact]
        public void SetsOutputIfSettingChanged()
        {
            ChangeSettingValue(nameof(Offset.Settings.Offset), 13.57);

            ProcessCycle();

            var outputValue = GetOutputValue<double>(nameof(Offset.ProcessDataOutputs.Value));
            outputValue.Should().Be(13.57);
        }
    }
}

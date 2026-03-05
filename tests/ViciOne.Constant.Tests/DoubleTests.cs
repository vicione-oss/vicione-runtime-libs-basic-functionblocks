using AwesomeAssertions;
using ViciOne.Constant;
using Xunit;

public class DoubleTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<Double>
    {
        [Fact]
        public void DoesNothingIfBlockIsNotEnabled()
        {
            var outputValueBefore = GetOutputValue<double>(nameof(Double.ProcessDataOutputs.Value));
            ChangeEnabled(false);
            ChangeSettingValue(nameof(Double.Settings.DefaultValue), outputValueBefore + 13.5);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<double>(nameof(Double.ProcessDataOutputs.Value));
            outputValueAfter.Should().Be(outputValueBefore);
        }

        [Theory]
        [InlineData(12.4)]
        [InlineData(-10)]
        public void PublishesSettingValueOnFirstRun(double settingValue)
        {
            ChangeSettingValue(nameof(Double.Settings.DefaultValue), settingValue);

            ProcessCycle();

            var outputValue = GetOutputValue<double>(nameof(Double.ProcessDataOutputs.Value));
            outputValue.Should().Be(settingValue);
        }

        [Theory]
        [InlineData(12.4)]
        [InlineData(-10)]
        public void SetsOutputIfSettingChangedAfterFirstRun(double settingValue)
        {
            ProcessCycle();
            ChangeSettingValue(nameof(Double.Settings.DefaultValue), settingValue);

            ProcessCycle();

            var outputValue = GetOutputValue<double>(nameof(Double.ProcessDataOutputs.Value));
            outputValue.Should().Be(settingValue);
        }
    }
}

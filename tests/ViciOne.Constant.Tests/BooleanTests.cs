using AwesomeAssertions;
using ViciOne.Constant;
using Xunit;

public class BooleanTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<Boolean>
    {
        [Fact]
        public void DoesNothingIfBlockIsNotEnabled()
        {
            var outputValueBefore = GetOutputValue<bool>(nameof(Boolean.ProcessDataOutputs.Value));
            ChangeEnabled(false);
            ChangeSettingValue(nameof(Boolean.Settings.DefaultValue), outputValueBefore == false);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<bool>(nameof(Boolean.ProcessDataOutputs.Value));
            outputValueAfter.Should().Be(outputValueBefore);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PublishesSettingValueOnFirstRun(bool settingValue)
        {
            ChangeSettingValue(nameof(Boolean.Settings.DefaultValue), settingValue);

            ProcessCycle();

            var outputValue = GetOutputValue<bool>(nameof(Boolean.ProcessDataOutputs.Value));
            outputValue.Should().Be(settingValue);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SetsOutputIfSettingChangedAfterFirstRun(bool settingValue)
        {
            ProcessCycle();
            ChangeSettingValue(nameof(Boolean.Settings.DefaultValue), settingValue);

            ProcessCycle();

            var outputValue = GetOutputValue<bool>(nameof(Boolean.ProcessDataOutputs.Value));
            outputValue.Should().Be(settingValue);
        }
    }
}

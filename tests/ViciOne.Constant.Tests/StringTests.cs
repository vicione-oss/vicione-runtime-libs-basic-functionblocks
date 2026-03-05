using AwesomeAssertions;
using ViciOne.Constant;
using Xunit;

public class StringTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<String>
    {
        [Fact]
        public void DoesNothingIfBlockIsNotEnabled()
        {
            var outputValueBefore = GetOutputValue<string>(nameof(String.ProcessDataOutputs.Value));
            ChangeEnabled(false);
            ChangeSettingValue(nameof(String.Settings.DefaultValue), outputValueBefore + "Sample Text");

            ProcessCycle();

            var outputValueAfter = GetOutputValue<string>(nameof(String.ProcessDataOutputs.Value));
            outputValueAfter.Should().Be(outputValueBefore);
        }

        [Theory]
        [InlineData("Sample Text 2")]
        [InlineData("Lorem ipsum dolor sit")]
        public void PublishesSettingValueOnFirstRun(string settingValue)
        {
            ChangeSettingValue(nameof(String.Settings.DefaultValue), settingValue);

            ProcessCycle();

            var outputValue = GetOutputValue<string>(nameof(String.ProcessDataOutputs.Value));
            outputValue.Should().Be(settingValue);
        }

        [Theory]
        [InlineData("Sample Text 2")]
        [InlineData("Lorem ipsum dolor sit")]
        public void SetsOutputIfSettingChangedAfterFirstRun(string settingValue)
        {
            ProcessCycle();
            ChangeSettingValue(nameof(String.Settings.DefaultValue), settingValue);

            ProcessCycle();

            var outputValue = GetOutputValue<string>(nameof(String.ProcessDataOutputs.Value));
            outputValue.Should().Be(settingValue);
        }
    }
}

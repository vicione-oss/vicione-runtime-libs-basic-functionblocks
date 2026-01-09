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
            ChangeEnabled(false);
            ProcessCycle();
            var outputValueBefore = GetOutputValue<bool>(nameof(Boolean.ProcessDataOutputs.Value));
            ChangeSettingValue(nameof(Boolean.Settings.DefaultValue), true);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<bool>(nameof(Boolean.ProcessDataOutputs.Value));
            outputValueAfter.Should().Be(outputValueBefore);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void SetsOutputIfSettingChanged(bool setting, bool expectedValue)
        {
            ChangeSettingValue(nameof(Boolean.Settings.DefaultValue), setting);

            ProcessCycle();

            var outputValue = GetOutputValue<bool>(nameof(Boolean.ProcessDataOutputs.Value));
            outputValue.Should().Be(expectedValue);
        }
    }
}

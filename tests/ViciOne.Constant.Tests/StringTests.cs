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
            ChangeEnabled(false);
            ProcessCycle();
            var outputValueBefore = GetOutputValue<string>(nameof(String.ProcessDataOutputs.Value));
            ChangeSettingValue(nameof(String.Settings.DefaultValue), "Sample Text");

            ProcessCycle();

            var outputValueAfter = GetOutputValue<string>(nameof(String.ProcessDataOutputs.Value));
            outputValueAfter.Should().Be(outputValueBefore);
        }

        [Theory]
        [InlineData("Sample Text 2", "Sample Text 2")]
        [InlineData("Lorem ipsum dolor sit", "Lorem ipsum dolor sit")]
        public void SetsOutputIfSettingChanged(string setting, string expectedValue)
        {
            ChangeSettingValue(nameof(String.Settings.DefaultValue), setting);

            ProcessCycle();

            var outputValue = GetOutputValue<string>(nameof(String.ProcessDataOutputs.Value));
            outputValue.Should().Be(expectedValue);
        }
    }
}

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
            ChangeEnabled(false);
            ProcessCycle();
            var outputValueBefore = GetOutputValue<double>(nameof(Double.ProcessDataOutputs.Value));
            ChangeSettingValue(nameof(Double.Settings.DefaultValue), 13.5);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<double>(nameof(Double.ProcessDataOutputs.Value));
            outputValueAfter.Should().Be(outputValueBefore);
        }

        [Theory]
        [InlineData(12.4, 12.4)]
        [InlineData(-10, -10)]
        public void SetsOutputIfSettingChanged(double setting, double expectedValue)
        {
            ChangeSettingValue(nameof(Double.Settings.DefaultValue), setting);

            ProcessCycle();

            var outputValue = GetOutputValue<double>(nameof(Double.ProcessDataOutputs.Value));
            outputValue.Should().Be(expectedValue);
        }
    }
}

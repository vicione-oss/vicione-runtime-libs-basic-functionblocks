using System;
using System.Globalization;
using AwesomeAssertions;
using AwesomeAssertions.Extensions;
using ViciOne.Time;
using Xunit;

public class SecondsSinceTests
{
    public class CalculateElapsedSeconds
    {
        [Fact]
        public void CalculatesElapsedSecondsCorrect()
        {
            DateTime startDate = new(2020, 1, 1, 0, 0, 0);
            DateTime endDate = new(2020, 1, 1, 0, 1, 13);

            var calculatedSeconds = SecondsSince.CalculateElapsedSeconds(startDate, endDate);

            calculatedSeconds.Should().Be(73);
        }
    }

    public class GetNow
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetsCorrectResult(bool useUtc)
        {
            var expected = useUtc ? DateTime.UtcNow : DateTime.Now;

            var result = SecondsSince.GetNow(useUtc);

            result.Should().BeIn(expected.Kind)
                .And.BeCloseTo(expected, 1.Seconds());
        }
    }

    public class ProcessCycle : RuntimeFunctionBlockTests<SecondsSince>
    {
        [Fact]
        public void CalculatesCorrectResults()
        {
            ChangeSettingValue(nameof(SecondsSince.Settings.StartDate), DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            var secondsBeforeCycle = DateTime.Now.TimeOfDay.TotalSeconds;

            ProcessCycle();

            var outputSecondsAfter = GetOutputValue<double>(nameof(SecondsSince.ProcessDataOutputs.Seconds));
            outputSecondsAfter.Should().BeInRange(secondsBeforeCycle, secondsBeforeCycle + 2);
        }

        [Fact]
        public void CalculatesNewResults()
        {
            var outputSecondsBefore = GetOutputValue<double>(nameof(SecondsSince.ProcessDataOutputs.Seconds));

            ProcessCycle();

            var outputSecondsAfter = GetOutputValue<double>(nameof(SecondsSince.ProcessDataOutputs.Seconds));
            outputSecondsAfter.Should().NotBe(outputSecondsBefore);
        }

        [Fact]
        public void DoesNotProcessOutputIfBlockIsNotEnabled()
        {
            var outputSecondsBefore = GetOutputValue<double>(nameof(SecondsSince.ProcessDataOutputs.Seconds));
            ChangeEnabled(false);

            ProcessCycle();

            var outputSecondsAfter = GetOutputValue<double>(nameof(SecondsSince.ProcessDataOutputs.Seconds));
            outputSecondsAfter.Should().Be(outputSecondsBefore);
        }

        [Fact]
        public void DoesNotProcessSettingIfBlockIsNotEnabled()
        {
            ChangeEnabled(false);
            ProcessCycle();
            var outputSecondsBefore = GetOutputValue<double>(nameof(SecondsSince.ProcessDataOutputs.Seconds));
            ChangeSettingValue(nameof(SecondsSince.Settings.StartDate), "1980-06-30");

            ProcessCycle();

            var outputSecondsAfter = GetOutputValue<double>(nameof(SecondsSince.ProcessDataOutputs.Seconds));
            outputSecondsAfter.Should().Be(outputSecondsBefore);
        }
    }
}

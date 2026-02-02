using System;
using AwesomeAssertions;
using AwesomeAssertions.Extensions;
using ViciOne.Time;
using Xunit;

public class CurrentDateTimeTests
{
    public class GetNow
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetsCorrectResult(bool useUtc)
        {
            var expected = useUtc ? DateTime.UtcNow : DateTime.Now;

            var result = CurrentDateTime.GetNow(useUtc);

            result.Should().BeIn(expected.Kind)
                .And.BeCloseTo(expected, 1.Seconds());
        }
    }

    public class ProcessCycle : RuntimeFunctionBlockTests<CurrentDateTime>
    {
        [Fact]
        public void PublishesCurrentTime()
        {
            var expectedKind = GetSettingValue<bool>(nameof(CurrentDateTime.Settings.UseUtc))
                ? DateTimeKind.Utc
                : DateTimeKind.Local;

            ProcessCycle();

            var value = GetOutputValue<DateTime>(nameof(CurrentDateTime.ProcessDataOutputs.Now));
            value.Kind.Should().Be(expectedKind);
            value.Should().BeCloseTo(CurrentDateTime.GetNow(expectedKind == DateTimeKind.Utc), 1.Seconds());
        }

        [Fact]
        public void PublishesNewValuesOverTime()
        {
            ProcessCycle();
            var first = GetOutputValue<DateTime>(nameof(CurrentDateTime.ProcessDataOutputs.Now));

            // ensure a time delta
            System.Threading.Thread.Sleep(20);

            ProcessCycle();
            var second = GetOutputValue<DateTime>(nameof(CurrentDateTime.ProcessDataOutputs.Now));

            second.Should().BeAfter(first);
        }

        [Fact]
        public void DoesNotPublishIfBlockIsNotEnabled()
        {
            ProcessCycle();
            var before = GetOutputValue<DateTime>(nameof(CurrentDateTime.ProcessDataOutputs.Now));

            ChangeEnabled(false);

            // ensure a time delta
            System.Threading.Thread.Sleep(20);

            ProcessCycle();
            var after = GetOutputValue<DateTime>(nameof(CurrentDateTime.ProcessDataOutputs.Now));

            after.Should().Be(before);
        }
    }
}

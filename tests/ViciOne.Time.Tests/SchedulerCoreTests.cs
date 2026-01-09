using System;
using System.Collections.Generic;
using System.Globalization;
using AwesomeAssertions;
using MELT;
using Microsoft.Extensions.Logging.Abstractions;
using ViciOne.Time;
using Xunit;

public class SchedulerCoreTests
{
    public class CheckForMatchingSchedule
    {
        [Fact]
        public void ReturnsTrueOnMatch()
        {
            var core = new SchedulerCore();
            core.ParseTimes("1#00:00", NullLoggerFactory.Instance.CreateLogger(""));

            core.CheckForMatchingSchedule(new DateTime(new DateOnly(2024, 06, 24), new TimeOnly(0, 0)))
                .Should().BeTrue();
        }

        [Fact]
        public void ReturnsFalseWhenNoScheduleForDayOfWeek()
        {
            var core = new SchedulerCore();

            core.CheckForMatchingSchedule(new DateTime(new DateOnly(2024, 06, 24), new TimeOnly(0, 0)))
                .Should().BeFalse();
        }

        [Fact]
        public void ReturnsFalseWhenNoMatch()
        {
            var core = new SchedulerCore();
            core.ParseTimes("2#00:00", NullLoggerFactory.Instance.CreateLogger(""));

            core.CheckForMatchingSchedule(new DateTime(new DateOnly(2024, 06, 24), new TimeOnly(0, 0)))
                .Should().BeFalse();
        }

        [Fact]
        public void ReturnsFalseOnSecondCallInOneMinute()
        {
            var core = new SchedulerCore();
            core.ParseTimes("1#00:00", NullLoggerFactory.Instance.CreateLogger(""));

            core.CheckForMatchingSchedule(new DateTime(new DateOnly(2024, 06, 24), new TimeOnly(0, 0)))
                .Should().BeTrue();

            core.CheckForMatchingSchedule(new DateTime(new DateOnly(2024, 06, 24), new TimeOnly(0, 0)))
                .Should().BeFalse();
        }
    }

    public class CreateTimeEntry
    {
        [Fact]
        public void CreatesTimeEntryCorrectly()
            => SchedulerCore.CreateTimeEntry(
                TimeSpan.FromHours(1)).Should().BeEquivalentTo(
                    new SchedulerCore.TimeEntry()
                    {
                        LastExecutedDate = DateTime.MinValue,
                        TimeOfDay = TimeSpan.FromHours(1)
                    });
    }

    public class ParseTimes
    {
        [Fact]
        public void ParsesTimeCorrectly()
        {
            Dictionary<DayOfWeek, List<SchedulerCore.TimeEntry>> expected = new()
            {
                [DayOfWeek.Monday] =
                [
                    new SchedulerCore.TimeEntry()
                    {
                        LastExecutedDate = DateTime.MinValue,
                        TimeOfDay = TimeSpan.ParseExact("00:00", "hh\\:mm", CultureInfo.InvariantCulture)
                    }
                ],
                [DayOfWeek.Tuesday] =
                [
                    new SchedulerCore.TimeEntry()
                    {
                        LastExecutedDate = DateTime.MinValue,
                        TimeOfDay = TimeSpan.ParseExact("01:00", "hh\\:mm", CultureInfo.InvariantCulture)
                    }
                ],
                [DayOfWeek.Wednesday] = [],
                [DayOfWeek.Thursday] = [],
                [DayOfWeek.Friday] = [],
                [DayOfWeek.Saturday] = [],
                [DayOfWeek.Sunday] = []
            };

            var act = new SchedulerCore()
                .ParseTimes("1#00:00;2#01:00", NullLoggerFactory.Instance.CreateLogger("nullLoger"));

            act.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void LogsInvalidCountOfEntryElements()
        {
            using var testLogger = TestLoggerFactory.Create();

            new SchedulerCore()
                .ParseTimes("1=00:00", testLogger.CreateLogger("testLogger"));

            testLogger.Sink.LogEntries.Should()
                .ContainSingle().Which.Message.Should()
                    .Be($"Invalid times configuration: Invalid count of elements in entry no 1 (Expected 2 elements, found 1 element).");
        }

        [Fact]
        public void LogsInvalidDayOfWeekFormat()
        {
            using var testLogger = TestLoggerFactory.Create();

            new SchedulerCore()
                .ParseTimes("A#00:00", testLogger.CreateLogger("testLogger"));

            testLogger.Sink.LogEntries.Should()
                .ContainSingle().Which.Message.Should()
                    .Be("Invalid times configuration: Invalid day of week format in entry no 1.");
        }

        [Fact]
        public void LogsInvalidDayOfWeekNumber()
        {
            using var testLogger = TestLoggerFactory.Create();

            new SchedulerCore()
                .ParseTimes("8#00:00", testLogger.CreateLogger("testLogger"));

            testLogger.Sink.LogEntries.Should()
                .ContainSingle().Which.Message.Should()
                    .Be("Invalid times configuration: Invalid day of week number in entry no 1.");
        }

        [Fact]
        public void LogsInvalidTimeSpan()
        {
            using var testLogger = TestLoggerFactory.Create();

            new SchedulerCore()
                .ParseTimes("1#61:99", testLogger.CreateLogger("testLogger"));

            testLogger.Sink.LogEntries.Should()
                .ContainSingle().Which.Message.Should()
                    .Be("Invalid times configuration: Invalid time of day format in entry no 1.");
        }
    }
}

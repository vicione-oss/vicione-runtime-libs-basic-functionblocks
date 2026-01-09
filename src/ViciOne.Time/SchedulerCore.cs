using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace ViciOne.Time;

internal sealed class SchedulerCore()
{
    private const string TimeOfDayFormat = "hh\\:mm";

    private static readonly char[] s_entryElementSplitter = ['#'];
    private static readonly char[] s_entrySplitter = [';'];

    private Dictionary<DayOfWeek, List<TimeEntry>> _times = [];
    private readonly Lock _timesLock = new();

    internal bool CheckForMatchingSchedule(DateTime now)
    {
        TimeEntry? entry;

        lock (_timesLock)
        {
            if (_times.TryGetValue(now.DayOfWeek, out List<TimeEntry>? times) is false)
                return false;

            var timeOfDayNow = new TimeSpan(0, now.TimeOfDay.Hours, now.TimeOfDay.Minutes, 0);

            entry = times.FirstOrDefault(x => x.TimeOfDay == timeOfDayNow && x.LastExecutedDate.Date < now.Date);
            if (entry is null)
                return false;
        }

        entry.LastExecutedDate = now.Date;

        return true;
    }

    internal static TimeEntry CreateTimeEntry(TimeSpan timeOfDay)
        => new()
        {
            TimeOfDay = timeOfDay,
            LastExecutedDate = DateTime.MinValue,
        };

    private static Dictionary<DayOfWeek, List<TimeEntry>> InitDayOfWeekDictionary()
        => new()
        {
            [DayOfWeek.Monday] = [],
            [DayOfWeek.Tuesday] = [],
            [DayOfWeek.Wednesday] = [],
            [DayOfWeek.Thursday] = [],
            [DayOfWeek.Friday] = [],
            [DayOfWeek.Saturday] = [],
            [DayOfWeek.Sunday] = []
        };

    internal Dictionary<DayOfWeek, List<TimeEntry>> ParseTimes(string? timesText, ILogger logger)
    {
        timesText ??= string.Empty;

        lock (_timesLock)
        {
            _times = InitDayOfWeekDictionary();
            string[] entries = timesText.Split(s_entrySplitter, StringSplitOptions.None);

            for (int i = 0; i < entries.Length; i++)
            {
                string entry = entries[i];
                string[] entryElements = entry.Split(s_entryElementSplitter, StringSplitOptions.None);
                if (entryElements.Length != 2)
                {
                    logger.InvalidTimesConfiguration_InvalidCountOfEntryElements(i + 1, entryElements.Length);
                    continue;
                }

                if (int.TryParse(entryElements[0], out int dayOfWeekNo) == false)
                {
                    logger.InvalidTimesConfiguration_InvalidDayOfWeekFormat(i + 1);
                    continue;
                }
                if (Enum.IsDefined(typeof(DayOfWeek), dayOfWeekNo) == false)
                {
                    logger.InvalidTimesConfiguration_InvalidDayOfWeekNumber(i + 1);
                    continue;
                }
                if (TimeSpan.TryParseExact(entryElements[1], TimeOfDayFormat, CultureInfo.InvariantCulture, out TimeSpan timeOfDay) == false)
                {
                    logger.InvalidTimesConfiguration_InvalidTimeOfDayFormat(i + 1);
                    continue;
                }

                _times[(DayOfWeek)dayOfWeekNo].Add(CreateTimeEntry(timeOfDay));
            }
        }

        return _times;
    }

    internal sealed class TimeEntry
    {
        public DateTime LastExecutedDate { get; set; }
        public TimeSpan TimeOfDay { get; init; }
    }
}

using Microsoft.Extensions.Logging;

namespace ViciOne.Time;

internal static partial class SchedulerCoreLog
{
    [LoggerMessage(Level = LogLevel.Error, Message = "Invalid times configuration: Invalid count of elements in entry no {EntryNo} (Expected 2 elements, found {FoundEntryElementCount} element).")]
    public static partial void InvalidTimesConfiguration_InvalidCountOfEntryElements(this ILogger logger, int entryNo, int foundEntryElementCount);

    [LoggerMessage(Level = LogLevel.Error, Message = "Invalid times configuration: Invalid day of week format in entry no {EntryNo}.")]
    public static partial void InvalidTimesConfiguration_InvalidDayOfWeekFormat(this ILogger logger, int entryNo);

    [LoggerMessage(Level = LogLevel.Error, Message = "Invalid times configuration: Invalid day of week number in entry no {EntryNo}.")]
    public static partial void InvalidTimesConfiguration_InvalidDayOfWeekNumber(this ILogger logger, int entryNo);

    [LoggerMessage(Level = LogLevel.Error, Message = "Invalid times configuration: Invalid time of day format in entry no {EntryNo}.")]
    public static partial void InvalidTimesConfiguration_InvalidTimeOfDayFormat(this ILogger logger, int entryNo);
}

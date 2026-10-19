using Microsoft.Extensions.Logging;

namespace ViciOne.Analysis;

internal static partial class IntervalStatisticLog
{
    [LoggerMessage(Level = LogLevel.Warning, Message = "{MissedIntervals} intervals were merged into longer ones because no engine cycle or scheduler tick occurred in time.")]
    public static partial void IntervalsMissed(this ILogger logger, long missedIntervals);
}

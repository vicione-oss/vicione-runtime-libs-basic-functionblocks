using ViciOne.Analysis;

/// <summary>
///     Counts how often an interval has elapsed.
/// </summary>
internal sealed class CountingIntervalElapsedHandler : IIntervalElapsedHandler
{
    public int Count { get; private set; }

    public long MissedIntervals { get; private set; }

    public void OnIntervalElapsed(long timestamp, long missedIntervals)
    {
        Count++;
        MissedIntervals += missedIntervals;
    }
}

namespace ViciOne.Analysis;

/// <summary>
/// Receiver of elapsed intervals.
/// </summary>
internal interface IIntervalElapsedHandler
{
    /// <summary>
    /// Called on the scheduler thread. Must be short and must not block the scheduler.
    /// </summary>
    /// <param name="timestamp">Time of the tick.</param>
    /// <param name="missedIntervals">Intervals that elapsed since the previous tick without being ticked.</param>
    void OnIntervalElapsed(long timestamp, long missedIntervals);
}

namespace ViciOne.Analysis;

/// <summary>
///     Receiver of elapsed intervals.
/// </summary>
internal interface IIntervalElapsedHandler
{
    /// <summary>
    ///     Called on the scheduler thread. Must be short and must not block the scheduler.
    /// </summary>
    void OnIntervalElapsed(long timestamp);
}

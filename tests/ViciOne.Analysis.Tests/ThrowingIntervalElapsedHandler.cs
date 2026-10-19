using System;
using ViciOne.Analysis;

/// <summary>
/// Throws on every elapsed interval.
/// </summary>
internal sealed class ThrowingIntervalElapsedHandler : IIntervalElapsedHandler
{
    public void OnIntervalElapsed(long timestamp, long missedIntervals)
        => throw new InvalidOperationException("Simulated failure");
}

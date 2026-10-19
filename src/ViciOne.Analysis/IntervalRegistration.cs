using System;

namespace ViciOne.Analysis;

/// <summary>
/// Registration of an <see cref="IIntervalElapsedHandler" /> at an <see cref="IntervalScheduler" />.
/// Disposing it unregisters the handler; afterwards the handler is no longer called.
/// </summary>
internal sealed class IntervalRegistration(IntervalScheduler scheduler, IntervalSchedulerGroup group, IIntervalElapsedHandler handler) : IDisposable
{
    internal bool ErrorReported { get; set; }

    internal IntervalSchedulerGroup Group { get; } = group;

    internal IIntervalElapsedHandler Handler { get; } = handler;

    /// <summary>
    /// Position within <see cref="Group" />; -1 once unregistered.
    /// </summary>
    internal int Index { get; set; } = -1;

    public void Dispose()
        => scheduler.Unregister(this);
}

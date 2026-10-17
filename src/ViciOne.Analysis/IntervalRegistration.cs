using System;

namespace ViciOne.Analysis;

/// <summary>
///     Registration of an <see cref="IIntervalElapsedHandler" /> at an <see cref="IntervalScheduler" />.
///     Disposing it unregisters the handler; afterwards the handler is no longer called.
/// </summary>
internal sealed class IntervalRegistration : IDisposable
{
    private readonly IntervalScheduler _scheduler;

    internal IntervalRegistration(IntervalScheduler scheduler, IntervalSchedulerGroup group, IIntervalElapsedHandler handler)
    {
        _scheduler = scheduler;
        Group = group;
        Handler = handler;
    }

    internal IntervalSchedulerGroup Group { get; }
    internal IIntervalElapsedHandler Handler { get; }

    /// <summary>
    ///     Position within <see cref="Group" />; -1 once unregistered.
    /// </summary>
    internal int Index { get; set; } = -1;

    public void Dispose()
        => _scheduler.Unregister(this);
}

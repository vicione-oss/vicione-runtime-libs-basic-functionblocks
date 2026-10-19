using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ViciOne.Analysis;

/// <summary>
/// All registrations of an <see cref="IntervalScheduler" /> that share the same interval.
/// </summary>
/// <remarks>
/// Not thread-safe; all members are accessed under the lock of the owning scheduler.
/// </remarks>
internal sealed class IntervalSchedulerGroup(long intervalMilliseconds, long period, long startTimestamp)
{
    private readonly List<IntervalRegistration> _members = [];

    public int Count => _members.Count;
    public long IntervalMilliseconds => intervalMilliseconds;
    public long NextDue { get; private set; } = startTimestamp + period;

    public void Add(IntervalRegistration registration)
    {
        registration.Index = _members.Count;
        _members.Add(registration);
    }

    /// <summary>
    /// Removes the registration in O(1) by moving the last member into its slot.
    /// </summary>
    public void Remove(IntervalRegistration registration)
    {
        int index = registration.Index;
        if (index < 0)
            return;

        int lastIndex = _members.Count - 1;
        IntervalRegistration last = _members[lastIndex];
        _members[index] = last;
        last.Index = index;
        _members.RemoveAt(lastIndex);
        registration.Index = -1;
    }

    public void Tick(long now)
    {
        long missedIntervals = (now - NextDue) / period;
        foreach (IntervalRegistration member in _members)
        {
            try
            {
                member.Handler.OnIntervalElapsed(now, missedIntervals);
            }
            catch (Exception exception)
            {
                if (member.ErrorReported == false)
                {
                    member.ErrorReported = true;
                    Trace.TraceError($"{nameof(IntervalScheduler)}: interval handler failed, further failures are not reported: {exception}");
                }
            }
        }

        NextDue += period;
        // Scheduler thread starved: do not catch up on all missed intervals.
        if (NextDue <= now)
            NextDue = now + period;
    }
}

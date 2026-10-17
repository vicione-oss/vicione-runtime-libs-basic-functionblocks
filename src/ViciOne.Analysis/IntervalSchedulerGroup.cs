using System;
using System.Collections.Generic;

namespace ViciOne.Analysis;

/// <summary>
///     All registrations of an <see cref="IntervalScheduler" /> that share the same interval.
/// </summary>
/// <remarks>
///     Not thread-safe; all members are accessed under the lock of the owning scheduler.
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
    ///     Removes the registration in O(1) by moving the last member into its slot.
    /// </summary>
    public void Remove(IntervalRegistration registration)
    {
        var index = registration.Index;
        if (index < 0)
            return;

        var lastIndex = _members.Count - 1;
        var last = _members[lastIndex];
        _members[index] = last;
        last.Index = index;
        _members.RemoveAt(lastIndex);
        registration.Index = -1;
    }

    public void Tick(long now)
    {
        foreach (var member in _members)
        {
            try
            {
                member.Handler.OnIntervalElapsed(now);
            }
            catch (Exception)
            {
                // A faulty receiver must not stop the remaining instances of the group.
            }
        }

        NextDue += period;
        // After a long stall (e.g. suspend), do not catch up on all missed intervals.
        if (NextDue <= now)
            NextDue = now + period;
    }
}

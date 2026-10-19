using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Loader;
using System.Threading;

namespace ViciOne.Analysis;

/// <summary>
/// Process-wide clock for all instances of <see cref="IntervalStatistic" />.
/// </summary>
/// <remarks>
/// All instances with the same interval form a group and are ticked together. A single background thread per load
/// context sleeps until the next due time (no polling). Due times advance at a fixed rate so the intervals do not
/// drift. Registering and unregistering are O(1).
/// The thread only runs while at least one group exists and ends when the load context of this assembly unloads.
/// </remarks>
internal sealed class IntervalScheduler
{
    private static readonly TimeSpan s_errorBackoff = TimeSpan.FromSeconds(1);

    private readonly List<IntervalSchedulerGroup> _dueGroups = [];
    private readonly Dictionary<long, IntervalSchedulerGroup> _groups = [];
    private readonly bool _runThread;
    // Deliberately not System.Threading.Lock: the worker uses Monitor.Wait to wait for new groups or the next due time.
    private readonly object _sync = new();
    private readonly bool _useStopwatch;
    private bool _threadRunning;

    /// <summary>
    /// Whether the background thread is currently running.
    /// </summary>
    internal bool IsThreadRunning
    {
        get
        {
            lock (_sync)
                return _threadRunning;
        }
    }

    public static IntervalScheduler Shared { get; } = new(TimeProvider.System, true);

    public TimeProvider TimeProvider { get; }

    public IntervalScheduler(TimeProvider timeProvider, bool runThread)
    {
        TimeProvider = timeProvider;
        _runThread = runThread;
        _useStopwatch = ReferenceEquals(timeProvider, TimeProvider.System);

        if (runThread && AssemblyLoadContext.GetLoadContext(typeof(IntervalScheduler).Assembly) is { } loadContext)
            loadContext.Unloading += _ => Clear();
    }

    /// <summary>
    /// Drops all groups and lets the thread end. Used when the load context unloads.
    /// </summary>
    internal void Clear()
    {
        lock (_sync)
        {
            _groups.Clear();
            Monitor.PulseAll(_sync);
        }
    }

    /// <summary>
    /// Current timestamp. In production it is read directly from <see cref="Stopwatch" />, because the virtual call
    /// through <see cref="TimeProvider" /> on every value change is measurable on small ARM devices.
    /// </summary>
    public long GetTimestamp()
        => _useStopwatch ? Stopwatch.GetTimestamp() : TimeProvider.GetTimestamp();

    public IntervalRegistration Register(long intervalMilliseconds, IIntervalElapsedHandler handler)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(intervalMilliseconds, 1);
        ArgumentNullException.ThrowIfNull(handler);

        lock (_sync)
        {
            if (_groups.TryGetValue(intervalMilliseconds, out IntervalSchedulerGroup? group) == false)
            {
                long period = intervalMilliseconds * TimeProvider.TimestampFrequency / 1000;
                group = new IntervalSchedulerGroup(intervalMilliseconds, Math.Max(1, period), GetTimestamp());
                _groups.Add(intervalMilliseconds, group);
                StartThreadIfRequired();
                Monitor.PulseAll(_sync);
            }

            var registration = new IntervalRegistration(this, group, handler);
            group.Add(registration);
            return registration;
        }
    }

    /// <summary>
    /// Ticks all due groups and returns the next due time (<see cref="long.MaxValue" /> if there is none).
    /// </summary>
    internal long RunDueGroups()
    {
        lock (_sync)
            return RunDueGroupsLocked();
    }

    private long RunDueGroupsLocked()
    {
        long now = GetTimestamp();
        long nextDue = long.MaxValue;
        _dueGroups.Clear();
        foreach (IntervalSchedulerGroup group in _groups.Values)
        {
            if (group.NextDue <= now)
                _dueGroups.Add(group);
            else
                nextDue = Math.Min(nextDue, group.NextDue);
        }

        foreach (IntervalSchedulerGroup group in _dueGroups)
        {
            group.Tick(now);
            nextDue = Math.Min(nextDue, group.NextDue);
        }

        _dueGroups.Clear();
        return nextDue;
    }

    private void StartThreadIfRequired()
    {
        if (_runThread == false || _threadRunning)
            return;

        _threadRunning = true;
        var thread = new Thread(ThreadWorker)
        {
            IsBackground = true,
            Name = nameof(IntervalScheduler),
        };
        thread.Start();
    }

    private void ThreadWorker()
    {
        lock (_sync)
        {
            try
            {
                while (_groups.Count > 0)
                {
                    try
                    {
                        long nextDue = RunDueGroupsLocked();
                        if (nextDue == long.MaxValue)
                            continue;

                        TimeSpan remaining = TimeProvider.GetElapsedTime(GetTimestamp(), nextDue);
                        if (remaining > TimeSpan.Zero)
                            Monitor.Wait(_sync, remaining + TimeSpan.FromMilliseconds(1));
                    }
                    catch (Exception exception)
                    {
                        Trace.TraceError($"{nameof(IntervalScheduler)}: {exception}");
                        Monitor.Wait(_sync, s_errorBackoff);
                    }
                }
            }
            finally
            {
                _threadRunning = false;
            }
        }
    }

    internal void Unregister(IntervalRegistration registration)
    {
        lock (_sync)
        {
            IntervalSchedulerGroup group = registration.Group;
            group.Remove(registration);
            if (group.Count == 0 && _groups.TryGetValue(group.IntervalMilliseconds, out IntervalSchedulerGroup? current) && ReferenceEquals(current, group))
            {
                _groups.Remove(group.IntervalMilliseconds);
                Monitor.PulseAll(_sync);
            }
        }
    }
}

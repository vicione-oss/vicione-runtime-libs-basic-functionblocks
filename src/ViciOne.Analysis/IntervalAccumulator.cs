using System;

namespace ViciOne.Analysis;

/// <summary>
///     Aggregates the values of an interval incrementally in O(1) per value and without allocations.
/// </summary>
/// <remarks>
///     Instead of storing every value in a list and evaluating it at the end of the interval, only the figures
///     (minimum, maximum, sum, time-weighted sum, first/second-to-last/last value) are kept up to date.
///     Not thread-safe: <see cref="Add" /> and <see cref="Complete" /> are called on the engine thread only.
///     Timestamps come from <see cref="IntervalScheduler.GetTimestamp" />.
/// </remarks>
internal sealed class IntervalAccumulator
{
    private int _count;
    private long _entryStart;
    private double _first;
    private bool _firstDifference = true;
    private long _intervalStart;
    private double _last;
    private double _maximum;
    private double _minimum;
    private double _secondLast;
    private double _sum;
    private double _weightedSum;

    /// <param name="initializationValue">Value present at the start; NaN means no value yet.</param>
    /// <param name="timestamp">Start of the first interval.</param>
    public IntervalAccumulator(double initializationValue, long timestamp)
    {
        StartInterval(double.IsNaN(initializationValue) ? 0 : initializationValue, timestamp);
        if (double.IsNaN(initializationValue))
            _count = 0;
    }

    /// <summary>
    ///     Records a new value. The previous value is weighted by the time it was present.
    /// </summary>
    public void Add(double value, long timestamp)
    {
        if (double.IsNaN(value))
            return;

        if (_count == 0)
        {
            StartInterval(value, timestamp);
            return;
        }

        _weightedSum += _last * (timestamp - _entryStart);
        _entryStart = timestamp;
        _secondLast = _last;
        _last = value;
        _count++;
        _sum += value;
        if (value < _minimum)
            _minimum = value;
        if (value > _maximum)
            _maximum = value;
    }

    /// <summary>
    ///     Completes the running interval at the given time and starts the next one with the last value.
    /// </summary>
    /// <remarks>
    ///     Completion happens in the first engine cycle after the tick. If a value was recorded in between, its timestamp
    ///     lies after the tick; the interval is then completed at the time of that value.
    /// </remarks>
    public IntervalResult Complete(long timestamp)
    {
        if (timestamp < _entryStart)
            timestamp = _entryStart;

        var result = Calculate(timestamp);
        if (_count > 1)
            _firstDifference = false;
        StartInterval(_last, timestamp);
        return result;
    }

    private IntervalResult Calculate(long timestamp)
    {
        var duration = timestamp - _intervalStart;
        var average = _count == 1 || duration <= 0
            ? _last
            : Math.Clamp((_weightedSum + (_last * (timestamp - _entryStart))) / duration, _minimum, _maximum);

        double difference;
        if (_firstDifference)
            difference = _count > 2 ? _first - _secondLast : 0;
        else
            difference = _first - _last;

        return new IntervalResult(average, _minimum, _maximum, _sum, difference, _last);
    }

    private void StartInterval(double value, long timestamp)
    {
        _count = 1;
        _first = value;
        _secondLast = value;
        _last = value;
        _minimum = value;
        _maximum = value;
        _sum = 0;
        _weightedSum = 0;
        _intervalStart = timestamp;
        _entryStart = timestamp;
    }
}

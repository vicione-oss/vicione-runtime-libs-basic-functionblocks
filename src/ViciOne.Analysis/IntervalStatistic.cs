using System.Threading;
using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Analysis;

/// <summary>
///     Aggregates the input value over a fixed interval (<c>CompressionTime</c>) into a time-weighted average, minimum,
///     maximum, sum and difference.
/// </summary>
/// <remarks>
///     The block runs in RunMode <c>Change</c>. Value changes are aggregated incrementally in <see cref="ProcessCycle" />.
///     The end of an interval is signalled by the process-wide <see cref="IntervalScheduler" />: it only stores the
///     timestamp and marks the block as changed. Completion and outputs follow in the next engine cycle on the engine
///     thread, so the aggregation needs no locks.
/// </remarks>
public partial class IntervalStatistic : IIntervalElapsedHandler
{
    private const long NoIntervalElapsed = long.MinValue;

    private IntervalAccumulator? _accumulator;
    private bool _dataReceived;
    private long _elapsedTimestamp = NoIntervalElapsed;
    private bool _passThroughInputValueAsAverage = true;
    private IntervalRegistration? _registration;

    internal IntervalScheduler Scheduler { get; set; } = IntervalScheduler.Shared;

    private void CreateAccumulator()
    {
        if (_accumulator is not null)
            return;

        var accumulator = new IntervalAccumulator(ProcessDataInputs.Value.Value, Scheduler.GetTimestamp());
        _registration = Scheduler.Register(Settings.CompressionTime.Value, this);
        _accumulator = accumulator;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
            DisposeAccumulator();
    }

    private void DisposeAccumulator()
    {
        // After unregistering, the scheduler no longer calls back; a tick still stored belongs to the old interval.
        _registration?.Dispose();
        _registration = null;
        _accumulator = null;
        Interlocked.Exchange(ref _elapsedTimestamp, NoIntervalElapsed);
    }

    void IIntervalElapsedHandler.OnIntervalElapsed(long timestamp)
    {
        Interlocked.Exchange(ref _elapsedTimestamp, timestamp);
        MarkAsChanged();
    }

    protected override void OnSettingChanged(IRuntimeSetting setting)
    {
        DisposeAccumulator();
        _passThroughInputValueAsAverage = true;
        if (_dataReceived)
            CreateAccumulator();
    }

    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
    {
        // An elapsed interval is always completed, but not written to the outputs while the block is disabled.
        var elapsedTimestamp = Interlocked.Exchange(ref _elapsedTimestamp, NoIntervalElapsed);
        if (elapsedTimestamp != NoIntervalElapsed && _accumulator is not null)
        {
            var result = _accumulator.Complete(elapsedTimestamp);
            if (FunctionBlockEnabled)
                SetResultToOutputs(result);
        }

        if (FunctionBlockEnabled == false)
            return;

        if (cycleInfo.IsFirstCycle && ProcessDataInputs.Value.Value == ProcessDataInputs.Value.Connector.Design.DefaultValue)
            return;

        if (ProcessDataInputs.Value.Changed || cycleInfo.IsFirstCycle)
        {
            if (_dataReceived == false)
            {
                _dataReceived = true;
                CreateAccumulator();
            }
            else
                _accumulator?.Add(ProcessDataInputs.Value.Value, Scheduler.GetTimestamp());
        }
    }

    internal void SetResultToOutputs(in IntervalResult result)
    {
        if (_dataReceived == false)
            return;

        // Like the previous implementation, Last and the pass-through average take the current input value, which may
        // differ from the last aggregated value (e.g. NaN, or a change while the block was disabled).
        if (_passThroughInputValueAsAverage)
        {
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Average, ProcessDataInputs.Value.Value);
            _passThroughInputValueAsAverage = false;
        }
        else
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Average, result.Average);

        SetConnectorValueIfNotEqual(ProcessDataOutputs.Difference, result.Difference);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.Last, ProcessDataInputs.Value.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.Maximum, result.Maximum);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.Minimum, result.Minimum);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.Sum, result.Sum);
    }
}

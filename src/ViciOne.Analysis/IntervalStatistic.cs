using ACX.ViciOne.Data.Logger;
using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Analysis;

public partial class IntervalStatistic
{
    private WeightedAverage? _average;
    private bool _dataReceived;
    private bool _passThroughInputValueAsAverage = true;

    private void AveragingTimeExpired(object? sender, AveragingTimeExpiredEventArgs e)
        => SetAveragingTimeToOutputs(e);

    private void CreateAverage()
    {
        if (_average is not null)
            return;

        _average = new WeightedAverage(Settings.CompressionTime.Value, ProcessDataInputs.Value.Value);
        _average.AveragingTimeExpired += AveragingTimeExpired;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
            DisposeAverage();
    }

    private void DisposeAverage()
    {
        if (_average is null)
            return;

        _average.AveragingTimeExpired -= AveragingTimeExpired;
        _average.Dispose();
        _average = null;
    }

    protected override void OnSettingChanged(IRuntimeSetting setting)
    {
        DisposeAverage();
        _passThroughInputValueAsAverage = true;
        if (_dataReceived)
            CreateAverage();
    }

    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
    {
        if (FunctionBlockEnabled && ProcessDataInputs.Value.Changed && ProcessDataInputs.Value.Value != ProcessDataInputs.Value.Connector.Design.DefaultValue)
        {
            if (_dataReceived == false)
            {
                _dataReceived = true;
                CreateAverage();
            }
            else
                _average?.AddValue(ProcessDataInputs.Value.Value);
        }
    }

    internal void SetAveragingTimeToOutputs(AveragingTimeExpiredEventArgs e)
    {
        if (FunctionBlockEnabled == false || _dataReceived == false)
            return;

        if (_passThroughInputValueAsAverage)
        {
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Average, ProcessDataInputs.Value.Value);
            _passThroughInputValueAsAverage = false;
        }
        else
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Average, e.Average);

        SetConnectorValueIfNotEqual(ProcessDataOutputs.Difference, e.Difference);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.Last, ProcessDataInputs.Value.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.Maximum, e.Maximum);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.Minimum, e.Minimum);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.Sum, e.Sum);
    }
}

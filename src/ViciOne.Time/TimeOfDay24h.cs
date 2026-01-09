using System;
using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Time;

public partial class TimeOfDay24h
{
    private void CalculateAndPublish()
    {
        if (FunctionBlockEnabled)
        {
            DateTime now = GetNow(Settings.UseUtc.Value);
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Hour, now.TimeOfDay.Hours);
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Minute, now.TimeOfDay.Minutes);
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Second, now.TimeOfDay.Seconds);
        }
    }

    internal static DateTime GetNow(bool useUtc)
        => useUtc
            ? DateTime.UtcNow
            : DateTime.Now;

    protected override void OnSettingChanged(IRuntimeSetting setting)
    {
        base.OnSettingChanged(setting);
        CalculateAndPublish();
    }

    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
        => CalculateAndPublish();
}

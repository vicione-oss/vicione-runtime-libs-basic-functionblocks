using System;
using System.Globalization;
using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Time;

public partial class SecondsSince
{
    private DateTime _startDate;

    private void CalculateAndPublish()
    {
        if (FunctionBlockEnabled)
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Seconds, CalculateElapsedSeconds(_startDate, GetNow(Settings.UseUtc.Value)));
    }

    internal static double CalculateElapsedSeconds(DateTime start, DateTime end)
        => end.Subtract(start).TotalSeconds;

    internal static DateTime GetNow(bool useUtc)
        => useUtc
            ? DateTime.UtcNow
            : DateTime.Now;

    protected override void OnSettingChanged(IRuntimeSetting setting)
    {
        base.OnSettingChanged(setting);

        _startDate = DateTime.ParseExact(Settings.StartDate.Value!,
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture,
            Settings.UseUtc.Value
                ? DateTimeStyles.AssumeUniversal
                : DateTimeStyles.AssumeLocal);

        if (Settings.UseUtc.Value)
            _startDate = _startDate.ToUniversalTime();

        CalculateAndPublish();
    }

    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
        => CalculateAndPublish();
}

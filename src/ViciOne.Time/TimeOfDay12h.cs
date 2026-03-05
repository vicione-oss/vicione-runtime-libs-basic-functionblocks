using System;
using System.Globalization;
using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Time;

public partial class TimeOfDay12h
{
    private void CalculateAndPublish()
    {
        if (FunctionBlockEnabled)
        {
            DateTime now = GetNow(Settings.UseUtc.Value);
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Hour, GetHour12h(now));
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Minute, now.TimeOfDay.Minutes);
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Second, now.TimeOfDay.Seconds);
            SetConnectorValueIfNotEqual(ProcessDataOutputs.AmPm, GetAmPmValue(now));
        }
    }

    internal static int GetAmPmValue(DateTime now)
    {
        string text = now.ToString("tt", CultureInfo.InvariantCulture);
        if (string.Equals(text, CultureInfo.InvariantCulture.DateTimeFormat.AMDesignator, StringComparison.Ordinal))
            return 0;
        else if (string.Equals(text, CultureInfo.InvariantCulture.DateTimeFormat.PMDesignator, StringComparison.Ordinal))
            return 1;
        else
            return -1;
    }

    internal static int GetHour12h(DateTime now)
        => int.Parse(now.ToString("hh", CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);

    internal static DateTime GetNow(bool useUtc)
        => useUtc
            ? DateTime.UtcNow
            : DateTime.Now;

    protected override void OnSettingChanged(IRuntimeSetting setting)
        => CalculateAndPublish();

    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
        => CalculateAndPublish();
}

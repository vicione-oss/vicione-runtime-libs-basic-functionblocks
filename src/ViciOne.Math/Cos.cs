using System;
using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Math;

public partial class Cos
{
    internal static double Calculate(double value, AngleUnit unit)
        => System.Math.Cos(ToRadians(value, unit));

    private void CalculateAndPublish()
    {
        if (FunctionBlockEnabled)
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Result, Calculate(ProcessDataInputs.Value.Value, Settings.InputUnit.Value));
    }

    protected override void OnSettingChanged(IRuntimeSetting setting)
    {
        base.OnSettingChanged(setting);
        CalculateAndPublish();
    }

    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
        => CalculateAndPublish();

    private static double ToRadians(double value, AngleUnit unit)
        => unit switch
        {
            AngleUnit.Radians => value,
            AngleUnit.Degrees => value * System.Math.PI / 180.0,
            _ => throw new NotImplementedException($"Unit '{unit}' is not implemented."),
        };
}

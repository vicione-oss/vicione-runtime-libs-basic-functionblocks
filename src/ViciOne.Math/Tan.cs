using System;
using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Math;

public partial class Tan
{
    internal static double Calculate(double value, AngleUnit unit)
    {
        double radians = ToRadians(value, unit);
        return ResultWillBeNaN(radians)
            ? double.NaN
            : System.Math.Tan(radians);
    }

    private void CalculateAndPublish()
    {
        if (FunctionBlockEnabled)
        {
            double result = Calculate(ProcessDataInputs.Value.Value, Settings.InputUnit.Value);
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Result, result);
            SetConnectorValueIfNotEqual(ProcessDataOutputs.NaN, double.IsNaN(result));
        }
    }

    protected override void OnSettingChanged(IRuntimeSetting setting)
    {
        base.OnSettingChanged(setting);
        CalculateAndPublish();
    }

    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
        => CalculateAndPublish();

    private static bool ResultWillBeNaN(double value)
        => (value + (System.Math.PI / 2)) % System.Math.PI == 0;

    private static double ToRadians(double value, AngleUnit unit)
        => unit switch
        {
            AngleUnit.Radians => value,
            AngleUnit.Degrees => value * System.Math.PI / 180.0,
            _ => throw new NotImplementedException($"Unit '{unit}' is not implemented."),
        };
}

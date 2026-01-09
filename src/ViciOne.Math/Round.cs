using System;
using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Math;

public partial class Round
{
    private void CalculateAndPublish()
    {
        if (FunctionBlockEnabled)
        {
            double value = RoundValue(ProcessDataInputs.Value.Value, Settings.DecimalPlaces.Value, Settings.RoundingBehavior.Value);
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Value, value);
        }
    }

    protected override void OnSettingChanged(IRuntimeSetting setting)
    {
        base.OnSettingChanged(setting);
        CalculateAndPublish();
    }

    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
        => CalculateAndPublish();

    internal static double RoundValue(double value, int decimalPlaces, RoundingBehavior behavior)
    {
        switch (behavior)
        {
            case RoundingBehavior.Commercial:
                return System.Math.Round(value, decimalPlaces, MidpointRounding.AwayFromZero);
            case RoundingBehavior.Up:
            case RoundingBehavior.Down:
                double roundedValue = value * System.Math.Pow(10, decimalPlaces);
                roundedValue = behavior == RoundingBehavior.Up ? System.Math.Ceiling(roundedValue) : System.Math.Floor(roundedValue);
                roundedValue *= System.Math.Pow(10, -1 * decimalPlaces);
                return System.Math.Round(roundedValue, decimalPlaces);
            default:
                return value;
        }
    }
}

using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Math;

public partial class Root
{
    private void CalculateAndPublish()
    {
        if (FunctionBlockEnabled)
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Value, System.Math.Pow(ProcessDataInputs.Radicand.Value, 1.0 / Settings.Index.Value));
    }

    protected override void OnSettingChanged(IRuntimeSetting setting)
    {
        base.OnSettingChanged(setting);
        CalculateAndPublish();
    }

    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
        => CalculateAndPublish();
}

using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Math;

public partial class Offset
{
    private void CalculateAndPublish()
    {
        if (FunctionBlockEnabled)
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Value, ProcessDataInputs.Value.Value + Settings.Offset.Value);
    }

    protected override void OnSettingChanged(IRuntimeSetting setting)
    {
        base.OnSettingChanged(setting);
        CalculateAndPublish();
    }

    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
        => CalculateAndPublish();
}

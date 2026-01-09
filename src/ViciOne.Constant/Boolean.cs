using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Constant;

public partial class Boolean
{
    private void CalculateAndPublish()
    {
        if (FunctionBlockEnabled)
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Value, Settings.DefaultValue.Value);
    }

    protected override void OnSettingChanged(IRuntimeSetting setting)
    {
        base.OnSettingChanged(setting);
        CalculateAndPublish();
    }
}

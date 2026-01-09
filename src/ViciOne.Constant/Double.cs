using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Constant;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Bezeichner enthält Typnamen", Justification = "Der FunctionBlock stellt eine Konstante genau dieses Datentyps dar.")]
public partial class Double
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

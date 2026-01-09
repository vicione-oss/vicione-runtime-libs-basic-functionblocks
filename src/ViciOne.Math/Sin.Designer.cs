using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;
using ViciOne.Engine.DefaultValidators;

namespace ViciOne.Math;

[FunctionBlockDesign("1851cf5b-4a04-4c2b-96f2-6a7a28a4906d", "6acb6ae6-7002-455d-8897-6600613ad4be", "3225b992-99a0-4891-acfd-17990a7cb736")]
public partial class Sin : RuntimeFunctionBlock<Sin>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("71697771-75a1-4a74-ba2e-ce0d747194d8")]
        public IRuntimeConnector<double> Value { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("290a71a1-ab85-45f7-9c30-80b7173cf23d")]
        public IRuntimeConnector<double> Result { get; internal set; }
    }

    public new Settings_ Settings { get; } = new Settings_();
    [SettingDesigns]
    public class Settings_
    {
        [SettingDesign("e9122b1f-9b86-4634-8528-b2d57bb29cf6", "11859f1e-eecc-4be7-aeb6-e78bd8854d94", "d607491c-3720-49e4-bcac-1f187bb0f500")]
        [EnumValidator<AngleUnit>(ViciOne.Math.AngleUnit.Radians)]
        public IRuntimeSetting<AngleUnit> InputUnit { get; internal set; }
    }
}

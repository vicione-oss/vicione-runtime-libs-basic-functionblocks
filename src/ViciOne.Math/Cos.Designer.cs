using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;
using ViciOne.Engine.DefaultValidators;

namespace ViciOne.Math;

[FunctionBlockDesign("6e857e7f-da96-40d4-9f40-1d944143bea7", "5a6888a3-6d97-4825-8298-43b0776f2560", "13d28e37-03b3-4d61-befb-7110c8ab987d")]
public partial class Cos : RuntimeFunctionBlock<Cos>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("e648e0eb-b024-41ff-adf2-38dd90426988")]
        public IRuntimeConnector<double> Value { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("c4d05895-1e7c-4faa-a4da-d0bb568ad923")]
        public IRuntimeConnector<double> Result { get; internal set; }
    }

    public new Settings_ Settings { get; } = new Settings_();
    [SettingDesigns]
    public class Settings_
    {
        [SettingDesign("8a668cdc-74e0-4b5c-92fa-018610a7c6a1", "6aa395ba-6c93-4cb8-b96d-58f06389672e", "f4f30c5d-abb8-4ec1-b51f-9cb54c39b08e")]
        [EnumValidator<AngleUnit>(ViciOne.Math.AngleUnit.Radians)]
        public IRuntimeSetting<AngleUnit> InputUnit { get; internal set; }
    }
}

using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;
using ViciOne.Engine.DefaultValidators;

namespace ViciOne.Math;

[FunctionBlockDesign("5f090543-e2b9-4540-af2b-8391dd2b787d", "76b17f48-82a8-4336-a38b-77e0c42abfb6", "8444e010-73d4-453d-a6c9-de48aaa83af0")]
public partial class Root : RuntimeFunctionBlock<Root>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("74afc24d-f1e0-48d7-b997-80878aad0504")]
        [RangeValidator<double>(0.0, double.MaxValue)]
        public IRuntimeConnector<double> Radicand { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("7c3e7673-9951-48c8-b9a3-9070b649589f")]
        public IRuntimeConnector<double> Value { get; internal set; }
    }

    public new Settings_ Settings { get; } = new Settings_();
    [SettingDesigns]
    public class Settings_
    {
        [SettingDesign("831a91b6-3887-41aa-aa6a-b2654acfb583", "91b9fb0b-90f6-49a2-b65a-3eae66739aee", "e389e575-c273-4d9b-b2c8-3ad1fa8049a6")]
        [DefaultValue(2.0)]
        [InequalityValidator<double>(0.0, 2.0)]
        public IRuntimeSetting<double> Index { get; internal set; }
    }
}

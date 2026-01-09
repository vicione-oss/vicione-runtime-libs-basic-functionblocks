using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;
using ViciOne.Engine.DefaultValidators;

namespace ViciOne.Math;

[FunctionBlockDesign("a65bf0ba-4da7-4c49-ad02-3c5f1f417ea7", "8bae80ba-1ca4-4763-a345-395dd74a267d", "e5b6fc4e-0901-423e-8ff7-95688e42ff72")]
public partial class Tan : RuntimeFunctionBlock<Tan>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("fde93d56-fb21-43cd-b482-079980d1487c")]
        public IRuntimeConnector<double> Value { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("4d70573d-93de-434d-9b96-90fef9055d4f")]
        public IRuntimeConnector<double> Result { get; internal set; }
        [OutputConnectorDesign("2cc3c039-cf7b-4d78-a285-44302c6c2e74")]
        public IRuntimeConnector<bool> NaN { get; internal set; }
    }

    public new Settings_ Settings { get; } = new Settings_();
    [SettingDesigns]
    public class Settings_
    {
        [SettingDesign("c7fd0751-a02f-4e4a-8c37-d65cf596e999", "809c96f4-0cad-43cc-9c8e-cdc2fb8977ba", "722b2a91-db16-4752-b4f8-05bebde11fbd")]
        [EnumValidator<AngleUnit>(ViciOne.Math.AngleUnit.Radians)]
        public IRuntimeSetting<AngleUnit> InputUnit { get; internal set; }
    }
}

using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Math;

[FunctionBlockDesign("bf5683ff-e13a-492c-a172-5531a7b01213", "10e240fd-eee2-447c-a877-e108309a38a1", "b66c93fd-90bc-448f-8ace-6934c3ca0478")]
public partial class Offset : RuntimeFunctionBlock<Offset>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("1eb485c2-9713-4b7e-9cd1-855ac615b2da")]
        public IRuntimeConnector<double> Value { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("993c0969-ca1b-4947-bff2-17e5aeaf428c")]
        public IRuntimeConnector<double> Value { get; internal set; }
    }

    public new Settings_ Settings { get; } = new Settings_();
    [SettingDesigns]
    public class Settings_
    {
        [SettingDesign("00e89cea-1b08-4516-a24a-70c080f34834", "4f52bc11-6f43-4e1a-b0c4-e6c8137a9d07", "23295475-de68-4408-b48e-743c31e7a072")]
        public IRuntimeSetting<double> Offset { get; internal set; }
    }
}

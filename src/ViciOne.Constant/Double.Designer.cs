using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Constant;

[FunctionBlockDesign("8107e079-2b19-43fc-8d97-8f9233acbf02", "500b56d4-2e74-4042-8d87-a91ed049cc0f", "9913cb47-93ed-46c2-aaaf-c9d8c49d3859")]
public partial class Double : RuntimeFunctionBlock<Double>
{
    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("54b47d6e-9868-4336-bd3f-85401e53a9b9")]
        public IRuntimeConnector<double> Value { get; internal set; }
    }

    public new Settings_ Settings { get; } = new Settings_();
    [SettingDesigns]
    public class Settings_
    {
        [SettingDesign("489570b9-112e-4668-9d44-444d80602784", "69a12df6-23f6-4989-8ee5-d3075e6998b6", "0406933c-bca2-43be-b244-5952c0fb04e5")]
        public IRuntimeSetting<double> DefaultValue { get; internal set; }
    }
}

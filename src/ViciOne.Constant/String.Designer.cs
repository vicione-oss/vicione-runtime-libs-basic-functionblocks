using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Constant;

[FunctionBlockDesign("e5490496-5e42-42f6-8930-3df23cbc384b", "f208f87d-3481-42fb-bf59-647fd6232b73", "e6136a1f-d93b-422e-b7aa-e6dc8451f1cb")]
public partial class String : RuntimeFunctionBlock<String>
{
    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("be435c3f-1d04-4857-acd4-a3ac8b9c27be")]
        public IRuntimeConnector<string> Value { get; internal set; }
    }

    public new Settings_ Settings { get; } = new Settings_();
    [SettingDesigns]
    public class Settings_
    {
        [SettingDesign("cfbbdf29-a565-48cc-b119-3a91bb5f8541", "032b9481-b39e-4ed6-a86c-7e763259414b", "f0db67ab-551c-4cc5-a9cc-b30d4bbb17a4")]
        [DefaultValue("")]
        public IRuntimeSetting<string> DefaultValue { get; internal set; }
    }
}

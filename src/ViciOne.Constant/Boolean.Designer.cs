using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Constant;

[FunctionBlockDesign("bd52fcc4-f317-4c18-9847-44c28c3a5222", "9596e04b-fae8-4603-928f-503c4ae91197", "1df23fb7-d03c-4da9-8f29-a43604a8ded0")]
public partial class Boolean : RuntimeFunctionBlock<Boolean>
{
    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("520d4dde-2f4a-4408-81c8-3e6a70781744")]
        public IRuntimeConnector<bool> Value { get; internal set; }
    }

    public new Settings_ Settings { get; } = new Settings_();
    [SettingDesigns]
    public class Settings_
    {
        [SettingDesign("ead355ed-8eb0-482f-859a-4dd2acfa2296", "49a39527-2b8a-4137-9e29-257ad680b333", "87d65347-7d98-425c-9bd1-fa7b3b3a0ed0")]
        public IRuntimeSetting<bool> DefaultValue { get; internal set; }
    }
}

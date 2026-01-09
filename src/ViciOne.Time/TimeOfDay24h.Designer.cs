using ViciOne.Core.Contracts.DataModel;
using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Time;

[FunctionBlockDesign("d06f78ee-d2a7-4bfd-868d-983df1395cdc", "6077c22e-c712-4ec0-8ccf-bb36ab327225", "5c11ef1a-aac6-425b-a4a1-1506b9a3f913")]
[DefaultRunMode(RunMode.Cyclic)]
[DefaultCycleFrequency(1)]
[ValidRunModes(RunMode.Cyclic)]
public partial class TimeOfDay24h : RuntimeFunctionBlock<TimeOfDay24h>
{
    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("d2a52bf2-2d57-4e8d-91ee-e8f4d3c9cc37")]
        public IRuntimeConnector<double> Hour { get; internal set; }
        [OutputConnectorDesign("a601240f-a0f2-445d-9471-1841f7a5f0a7")]
        public IRuntimeConnector<double> Minute { get; internal set; }
        [OutputConnectorDesign("2629093f-b8cb-4630-86eb-282600212027")]
        public IRuntimeConnector<double> Second { get; internal set; }
    }

    public new Settings_ Settings { get; } = new Settings_();
    [SettingDesigns]
    public class Settings_
    {
        [SettingDesign("0c847c48-c20c-4770-a8bb-7427b62761e9", "b69334eb-2fcd-42f6-b4e5-c3f50194b374", "dedc25de-23af-47fc-aa97-49b387358b68")]
        public IRuntimeSetting<bool> UseUtc { get; internal set; }
    }
}

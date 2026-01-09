using ViciOne.Core.Contracts.DataModel;
using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Time;

[FunctionBlockDesign("ae0cc574-126a-4f07-853d-136184358d3e", "2c065b41-223b-430b-86ab-eaca5be806e9", "37dcee16-efe7-4922-acb2-350be9a2f0ec")]
[DefaultRunMode(RunMode.Cyclic)]
[DefaultCycleFrequency(1)]
[ValidRunModes(RunMode.Cyclic)]
public partial class TimeOfDay12h : RuntimeFunctionBlock<TimeOfDay12h>
{
    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("a44a94bf-a44d-4919-ad43-c3bcf8321eb6")]
        public IRuntimeConnector<double> Hour { get; internal set; }
        [OutputConnectorDesign("6f2fe57f-ec67-4022-93d7-197abadc22d1")]
        public IRuntimeConnector<double> Minute { get; internal set; }
        [OutputConnectorDesign("b37a4e43-afd1-4048-94d8-ddcc3400f8f3")]
        public IRuntimeConnector<double> Second { get; internal set; }
        [OutputConnectorDesign("cf68933b-4e33-4019-a2cb-078a7743ad22")]
        public IRuntimeConnector<double> AmPm { get; internal set; }
    }

    public new Settings_ Settings { get; } = new Settings_();
    [SettingDesigns]
    public class Settings_
    {
        [SettingDesign("93e568d0-01d9-4c0d-a887-b2b8789467c3", "92a8f4f1-eb79-4958-9c3d-97e85a769b22", "4aec059b-022b-4a2b-8839-2d72ce7069b2")]
        public IRuntimeSetting<bool> UseUtc { get; internal set; }
    }
}

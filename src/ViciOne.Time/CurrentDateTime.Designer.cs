using System;
using ViciOne.Core.Contracts.DataModel;
using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Time;

[FunctionBlockDesign("48f12e2d-5321-4a79-a5eb-f710632cc5ef", "a227f01f-dbfb-4a41-b5e0-b66b97374ba9", "7a0399fa-a0ca-45ef-957a-9d6e7dc0c3cb")]
[DefaultRunMode(RunMode.Cyclic)]
[DefaultCycleFrequency(1)]
[ValidRunModes(RunMode.Cyclic)]
public partial class CurrentDateTime : RuntimeFunctionBlock<CurrentDateTime>
{
    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("16699d8b-6bd5-4f14-b5ab-0dc68600de54")]
        public IRuntimeConnector<DateTime> Now { get; internal set; }
    }

    public new Settings_ Settings { get; } = new Settings_();
    [SettingDesigns]
    public class Settings_
    {
        [SettingDesign("2e19d86c-3dc8-49ab-9d01-d5b2dc36e399", "893dc9e9-39c4-4a95-a3ff-9675b1ab1412", "5c81441f-69b0-49a0-b992-5f1dd7b8bfac")]
        public IRuntimeSetting<bool> UseUtc { get; internal set; }
    }
}

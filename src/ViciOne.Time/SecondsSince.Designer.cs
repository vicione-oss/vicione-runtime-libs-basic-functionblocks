using ViciOne.Core.Contracts.DataModel;
using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;
using ViciOne.Engine.DefaultValidators;

namespace ViciOne.Time;

[FunctionBlockDesign("4096d52b-908c-4ff9-9226-59d842aba413", "ce86b1fc-6686-4474-998f-c865a3b32d98", "b891d9e7-e89e-44f4-86b8-7dbddf6cdc7e")]
[DefaultRunMode(RunMode.Cyclic)]
[DefaultCycleFrequency(1)]
[ValidRunModes(RunMode.Cyclic)]
public partial class SecondsSince : RuntimeFunctionBlock<SecondsSince>
{
    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("0ebb9ac1-cc87-408c-919e-c33f4efd0f81")]
        public IRuntimeConnector<double> Seconds { get; internal set; }
    }

    public new Settings_ Settings { get; } = new Settings_();
    [SettingDesigns]
    public class Settings_
    {
        [SettingDesign("6fcc7311-bba7-474d-abbd-ff2d327de5f2", "9f298e5c-1e30-4f98-b2f7-8f48d124d5dc", "82473330-edf4-4bed-813f-176198750d42")]
        public IRuntimeSetting<bool> UseUtc { get; internal set; }
        [SettingDesign("be68c986-dc70-4331-a7f7-fb884cf6ada4", "f26c19fc-806e-4458-abe6-cd3873106855", "5cfff396-75f8-43d6-a3d2-212ae8812ae9")]
        [DefaultValue("1970-01-01")]
        [StringLengthValidator(10, 10)]
        public IRuntimeSetting<string> StartDate { get; internal set; }
    }
}

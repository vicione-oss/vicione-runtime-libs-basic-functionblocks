using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Time;

[FunctionBlockDesign("9f7e3f96-93ff-470c-b550-20492a749659", "0d3270aa-e226-49c0-9251-89871c128b76", "5e0612cb-789f-49f3-9c01-57ac02b865e9")]
public partial class Scheduler : RuntimeFunctionBlock<Scheduler>
{
    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("713c5651-a5a7-40df-8232-b3265dba32ec")]
        public IRuntimeConnector<bool> Trigger { get; internal set; }
    }

    public new Settings_ Settings { get; } = new Settings_();
    [SettingDesigns]
    public class Settings_
    {
        [SettingDesign("ffc710a1-e01d-4202-9d1c-7445c5505aa6", "c5be6f53-d707-499c-bb7e-5d480fd033b0", "a574776b-8734-4338-a344-f9c27fd2e7de")]
        public IRuntimeSetting<string> Times { get; internal set; }
    }
}

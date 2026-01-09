using ViciOne.Core.Contracts.DataModel;
using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Math;

[FunctionBlockDesign("20a75996-9bf4-403b-9ea5-b3b85f973d5b", "9a830f6b-d969-4ebc-91fc-6d2134818726", "7f11a578-348f-4653-aab9-e3ae5106dc58")]
[DefaultRunMode(RunMode.Change)]
[ValidRunModes(RunMode.Change, RunMode.Cyclic)]
public partial class Random : RuntimeFunctionBlock<Random>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("a3b9e2ae-1fa6-4631-9b0a-7ccd2456925d")]
        public IRuntimeConnector<bool> Trigger { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("65768e50-b5fa-4434-9b66-9db0f7a42b62")]
        public IRuntimeConnector<long> NextLong { get; internal set; }
        [OutputConnectorDesign("31af4345-387d-4555-8796-ed8d5a96b3e7")]
        public IRuntimeConnector<double> NextDouble { get; internal set; }
    }

    public new Settings_ Settings { get; } = new Settings_();
    [SettingDesigns]
    public class Settings_
    {
        [SettingDesign("1b1ad5fa-44a1-4e18-bce2-935fdfe4a811", "fa0cac11-8e60-4f0d-a4e1-8650efa3a085", "cfa18d36-d44b-45c5-a303-63af586b4f65")]
        [DefaultValue(int.MinValue)]
        public IRuntimeSetting<int> Minimum { get; internal set; }
        [SettingDesign("b9f1b8d9-74fa-440c-9bb1-338074b939e6", "b27d13a3-7464-48ce-872d-a2e1085cec57", "67375eef-417f-4ccc-8c1a-eb2da4222b93")]
        [DefaultValue(int.MaxValue)]
        public IRuntimeSetting<int> Maximum { get; internal set; }
    }
}

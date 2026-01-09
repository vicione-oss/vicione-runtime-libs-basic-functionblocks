using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;
using ViciOne.Engine.DefaultValidators;

namespace ViciOne.Analysis;

[FunctionBlockDesign("b7177184-76db-4b58-93b8-bd238ac1e277", "3226c055-4aae-4315-b096-ebdfac1248d3", "b2aa8f18-cf4f-4ce1-be6b-29d1e76ea566")]
public partial class IntervalStatistic : RuntimeFunctionBlock<IntervalStatistic>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("4f2dffe7-b12e-4798-a80b-57acd8093a38")]
        [DefaultValue(double.MinValue)]
        public IRuntimeConnector<double> Value { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("700def8a-5ea1-47c2-8e8d-14fb3172a6d2")]
        public IRuntimeConnector<double> Average { get; internal set; }
        [OutputConnectorDesign("ffb7474d-df41-4bd1-9220-5aaf31aec334")]
        public IRuntimeConnector<double> Difference { get; internal set; }
        [OutputConnectorDesign("bf07d241-d366-48d2-b14d-0ebb4a289732")]
        public IRuntimeConnector<double> Last { get; internal set; }
        [OutputConnectorDesign("8a3d9802-a9e6-40f7-a034-ee9e5872f806")]
        public IRuntimeConnector<double> Maximum { get; internal set; }
        [OutputConnectorDesign("60c2ecd6-ea63-446d-8ab2-014476beb999")]
        public IRuntimeConnector<double> Minimum { get; internal set; }
        [OutputConnectorDesign("ab18d735-de39-4041-833a-47c3c5bd44fd")]
        public IRuntimeConnector<double> Sum { get; internal set; }
    }

    public new Settings_ Settings { get; } = new Settings_();
    [SettingDesigns]
    public class Settings_
    {
        [SettingDesign("ac25b1ff-6683-401f-bb4d-6a7a0578742c", "03a77131-5e80-46c5-b3ba-ffdba9848398", "03a77131-5e80-46c5-b3ba-ffdba9848398")]
        [DefaultValue(10000)]
        [RangeValidator<int>(1, int.MaxValue)]
        public IRuntimeSetting<int> CompressionTime { get; internal set; }
    }
}

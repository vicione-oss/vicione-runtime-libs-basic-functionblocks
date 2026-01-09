using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Math;

[FunctionBlockDesign("1eeeb8fb-c0c5-405c-be17-5d95bd34bf98", "ba4e0644-ddac-48c6-99fb-335e3d1c6aa4", "eb3f0d49-d4c9-41c5-86cf-a5cc766c5078")]
public partial class Equality : RuntimeFunctionBlock<Equality>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("c45f2392-95c8-4ff9-befb-c78ea8db4834")]
        public IRuntimeConnector<double> Double0 { get; internal set; }
        [InputConnectorDesign("f3bb683d-3d15-49b6-9529-f1f8e79a4e3c")]
        public IRuntimeConnector<double> Double1 { get; internal set; }
        [InputConnectorDesign("15359b81-8a80-47f5-b7ce-19e23961624a")]
        public IRuntimeConnector<long> Long0 { get; internal set; }
        [InputConnectorDesign("3c782bbd-535a-484c-a2f7-3474af8ddfec")]
        public IRuntimeConnector<long> Long1 { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("8da861dd-b0ea-4a71-9fdb-88c7f4015526")]
        public IRuntimeConnector<bool> Equal { get; internal set; }
        [OutputConnectorDesign("6d58f3aa-bd63-4887-9f1a-bd7097a9f784")]
        public IRuntimeConnector<bool> Unequal { get; internal set; }
        [OutputConnectorDesign("ea244556-963f-4d43-9d94-f7eaf42e0d3f")]
        public IRuntimeConnector<bool> GreaterThan { get; internal set; }
        [OutputConnectorDesign("7b0e800a-d18c-49af-a4a2-214555836bb1")]
        public IRuntimeConnector<bool> LessThan { get; internal set; }
    }
}

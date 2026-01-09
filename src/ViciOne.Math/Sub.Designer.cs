using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Math;

[FunctionBlockDesign("59267a8b-d4ae-4870-82ff-3ff8f43c7f78", "83611397-6f03-40b0-98aa-41f3ab8718e7", "f7beed66-f0fd-4dbb-8339-62566bd6c908")]
public partial class Sub : RuntimeFunctionBlock<Sub>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("c1d54211-6087-43ba-a91e-e28d533a0eec")]
        public IRuntimeConnector<double> Minuend { get; internal set; }
        [InputConnectorDesign("1d340a21-d7d2-4e96-8d3f-7eadf80e77ec")]
        public IRuntimeConnector<double> Subtrahend { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("b152254c-bb74-41ed-a363-a271962734aa")]
        public IRuntimeConnector<double> Difference { get; internal set; }
    }
}

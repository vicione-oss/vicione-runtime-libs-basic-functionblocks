using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Logic;

[FunctionBlockDesign("594f9cf9-3c38-4dc6-b899-a1dee6aa573d", "a4aa527a-899d-4431-97c0-bb8af8075aba", "326e2404-f461-4e2e-8c5f-1e988a04ad34")]
public partial class XNOR : RuntimeFunctionBlock<XNOR>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("2579e111-2ea9-47b6-b428-48f8d3bbdfd6")]
        public IRuntimeConnector<bool> A { get; internal set; }
        [InputConnectorDesign("e64e63a9-4a8a-4eb6-a560-03499cf86e7e")]
        public IRuntimeConnector<bool> B { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("5cc8c4df-c19b-4b5f-aec5-668c6a66894c")]
        public IRuntimeConnector<bool> Y { get; internal set; }
    }
}

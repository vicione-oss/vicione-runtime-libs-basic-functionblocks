using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Logic;

[FunctionBlockDesign("16f579fa-59c8-41b7-9ad2-d18a4f196903", "d8758532-ca06-4394-a72e-eac2fb64f5e5", "781c95db-cf9a-4ecd-8c9f-03166f9c1812")]
public partial class AND : RuntimeFunctionBlock<AND>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("612fa41a-6dc1-480f-bf65-a93519af34bf")]
        public IRuntimeConnector<bool> A { get; internal set; }
        [InputConnectorDesign("c9f3874e-21b4-4e51-86b3-dd5fcdf73fac")]
        public IRuntimeConnector<bool> B { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("bc2ab17e-5275-41ca-b9ac-46dc493f2e79")]
        public IRuntimeConnector<bool> Y { get; internal set; }
    }
}

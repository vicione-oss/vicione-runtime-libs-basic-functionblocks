using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Math;

[FunctionBlockDesign("09ce46e5-7875-4aa4-8c6a-8f729d969b8c", "871613ec-4f04-4cb2-8a8b-56e55b666f17", "a3b17c42-0eb6-4fbf-96a0-3376cce7dc79")]
public partial class Ceiling : RuntimeFunctionBlock<Ceiling>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("d4d00c37-2dc1-4438-bff3-e53072808787")]
        public IRuntimeConnector<double> Value { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("4bc7048d-cb2a-45e6-a23b-444509084723")]
        public IRuntimeConnector<double> Ceiling { get; internal set; }
    }
}

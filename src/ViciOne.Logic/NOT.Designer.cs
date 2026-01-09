using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Logic;

[FunctionBlockDesign("6c3ed0d2-ab9a-40fe-bb75-3d3036653c63", "256a4b6e-26aa-4454-b3e4-385174edd528", "2963e8e9-0fb0-42de-a8f2-7e8994e24e10")]
public partial class NOT : RuntimeFunctionBlock<NOT>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("520b63b3-08d8-47e7-bc73-4a3369713b06")]
        public IRuntimeConnector<bool> A { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("59b556e7-283a-40b4-afbc-21c500064cba")]
        public IRuntimeConnector<bool> Y { get; internal set; }
    }
}

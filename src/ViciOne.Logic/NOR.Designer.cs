using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Logic;

[FunctionBlockDesign("1ebbec88-f36c-4b5c-a2d3-d365aa0a441d", "690083d6-cc0f-40de-9a99-e8d21b0e4ab3", "aa4b348c-af06-4eb0-8bf0-b3b50579e3f3")]
public partial class NOR : RuntimeFunctionBlock<NOR>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("941ef150-5c09-4d80-a014-6388b60f3ea1")]
        public IRuntimeConnector<bool> A { get; internal set; }
        [InputConnectorDesign("18a6cbd1-5ba2-4eee-8a5c-e1d5a002df57")]
        public IRuntimeConnector<bool> B { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("968478bb-2b33-4d9e-b015-bcd20c1ad472")]
        public IRuntimeConnector<bool> Y { get; internal set; }
    }
}

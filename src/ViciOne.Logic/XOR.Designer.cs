using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Logic;

[FunctionBlockDesign("91bcf6ec-5725-488d-a006-a701e6d24079", "7e8f637d-37dc-49ac-abb4-f85fc6f139aa", "65ce63b5-f4c2-4398-983a-4fb4e3b4e6ae")]
public partial class XOR : RuntimeFunctionBlock<XOR>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("9614654c-b439-47f5-a1d2-b72439ca1a1e")]
        public IRuntimeConnector<bool> A { get; internal set; }
        [InputConnectorDesign("b4776f43-6041-4d5a-be47-aa635a85f015")]
        public IRuntimeConnector<bool> B { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("1961d6d8-ff52-41c8-ac3d-aa379a75da79")]
        public IRuntimeConnector<bool> Y { get; internal set; }
    }
}

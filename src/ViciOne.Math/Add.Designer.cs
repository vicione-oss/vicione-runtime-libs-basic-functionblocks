using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Math;

[FunctionBlockDesign("8172c8ba-07a0-4466-9bad-d577bd835f25", "517d27ec-6365-4fd9-bdc3-3a411cf10309", "e9b98144-87dc-4a3c-aa54-9d65e1676773")]
public partial class Add : RuntimeFunctionBlock<Add>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("dc769b21-08eb-4f5f-ae3e-f4857673380e")]
        public IRuntimeConnector<double> Addend1 { get; internal set; }
        [InputConnectorDesign("9a015a36-af88-4880-8992-c94b899b5b87")]
        public IRuntimeConnector<double> Addend2 { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("beb9394e-098a-4d80-bbd8-1378af1f58a6")]
        public IRuntimeConnector<double> Sum { get; internal set; }
    }
}

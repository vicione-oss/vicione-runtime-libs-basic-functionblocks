using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Logic;

[FunctionBlockDesign("d8a4c4db-40af-4ac5-a1c9-e741231c2c27", "b26af31c-e46e-433d-8c9b-50ebca66e048", "a7823aa6-7b29-430a-ae0f-dccb57fad6e5")]
public partial class NAND : RuntimeFunctionBlock<NAND>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("dd5e18d5-a52c-4bb2-952d-2c57838e082a")]
        public IRuntimeConnector<bool> A { get; internal set; }
        [InputConnectorDesign("5c563c48-af49-4761-94f3-38f8f395fe1f")]
        public IRuntimeConnector<bool> B { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("90e41ed3-4e79-4d23-ad15-ed58c1273b21")]
        public IRuntimeConnector<bool> Y { get; internal set; }
    }
}

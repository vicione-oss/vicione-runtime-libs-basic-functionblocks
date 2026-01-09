using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Math;

[FunctionBlockDesign("33d376a7-3f2e-45a4-bd9d-5ffdecf298e1", "9923c21f-f452-47bb-8d19-7dc987c9287c", "a5882889-f4ce-4b5d-9338-bfb412600c67")]
public partial class Exp : RuntimeFunctionBlock<Exp>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("8e891a06-9f2d-4ece-ab33-4c6d628d83f1")]
        public IRuntimeConnector<double> X { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("ff2e4a4c-e97e-40cf-8c8d-36815610d486")]
        public IRuntimeConnector<double> Exp { get; internal set; }
    }
}

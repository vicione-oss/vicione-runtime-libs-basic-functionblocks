using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Math;

[FunctionBlockDesign("95e35e3a-4d92-43e9-9eef-06c86bb6ac36", "f95cdc65-bf97-4515-9ddc-0516f35f8d82", "7dd840aa-d90e-4b1a-ae16-cfd696057658")]
public partial class Log : RuntimeFunctionBlock<Log>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("87fe3df6-3ded-4eb8-93f2-1dd33783a83c")]
        public IRuntimeConnector<double> X { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("2ff9f84e-1ed3-4a53-9c82-fd3bbaf5b665")]
        public IRuntimeConnector<double> Log { get; internal set; }
    }
}

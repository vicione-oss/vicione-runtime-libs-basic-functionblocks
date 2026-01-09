using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Math;

[FunctionBlockDesign("0501f6b7-6319-4cd6-b749-be23522a5f6b", "fc26f070-b122-4560-af76-aa9860551f0b", "eb599f74-380d-475b-bb2a-7c49c7cda485")]
public partial class Mul : RuntimeFunctionBlock<Mul>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("32e49198-884f-481b-810a-93f51127da0f")]
        public IRuntimeConnector<double> Factor1 { get; internal set; }
        [InputConnectorDesign("096465f1-8967-4cdf-8afd-518f6a1ad6ed")]
        public IRuntimeConnector<double> Factor2 { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("3b38608c-4da3-48c6-b743-634d4f1433ec")]
        public IRuntimeConnector<double> Product { get; internal set; }
    }
}

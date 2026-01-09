using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Math;

[FunctionBlockDesign("163099e0-e2b5-4f16-afeb-0ad4cccdb4d9", "235d8d5b-587e-42a2-ac11-69352f6124e9", "deebd786-5150-4300-aedb-91ab14245644")]
public partial class AbsDiff : RuntimeFunctionBlock<AbsDiff>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("c623f76c-94cc-4e3e-9357-71ca3da9ad6a")]
        public IRuntimeConnector<double> Ax { get; internal set; }
        [InputConnectorDesign("19502ddd-4e0b-4a3e-8c68-df077366f294")]
        public IRuntimeConnector<double> Ay { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("3388c094-8a76-4b54-b339-0e6288027db6")]
        public IRuntimeConnector<double> Diff { get; internal set; }
    }
}

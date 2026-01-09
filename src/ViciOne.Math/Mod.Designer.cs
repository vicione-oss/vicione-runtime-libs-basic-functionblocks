using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Math;

[FunctionBlockDesign("ca31c27c-c9b6-4067-a6f1-2af9051b1b53", "bc1a481c-d613-4eda-8ed3-abd515aaa1b4", "183f035c-e551-4ca7-8d29-82e7ab2fc148")]
public partial class Mod : RuntimeFunctionBlock<Mod>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("29b43630-779f-44aa-bfe0-8c3df7c5ad8b")]
        public IRuntimeConnector<double> Dividend { get; internal set; }
        [InputConnectorDesign("cb7c71da-d8ed-421e-a041-6b11486b4781")]
        public IRuntimeConnector<double> Divisor { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("59783439-d95b-40cd-a65f-9bf2d95a09f9")]
        public IRuntimeConnector<double> Remainder { get; internal set; }
    }
}

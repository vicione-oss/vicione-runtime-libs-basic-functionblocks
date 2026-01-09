using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Converter;

[FunctionBlockDesign("08dc7173-c33f-49bf-83a4-3c001b450380", "15be4231-816c-462e-98b8-d7dc2ae77ce8", "9947518b-9b0e-4b2a-a9a3-a0b00af414c1")]
public partial class LongToDouble : RuntimeFunctionBlock<LongToDouble>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("90608468-d669-4380-ac1f-fab7c6af83a0")]
        public IRuntimeConnector<long> Value { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("fafaca49-e805-4262-8d04-d190981f1cd3")]
        public IRuntimeConnector<double> Value { get; internal set; }
    }
}

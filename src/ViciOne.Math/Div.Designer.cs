using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Math;

[FunctionBlockDesign("5e039767-e746-4e4f-a68e-fa07ab980b8f", "33372369-519e-4bd8-ac9b-9cb0f986aeb8", "e3908eed-5b07-42b1-9b25-5e8793f19ada")]
public partial class Div : RuntimeFunctionBlock<Div>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("3c9ae16a-801e-44f6-ab30-9002e579248e")]
        public IRuntimeConnector<double> Dividend { get; internal set; }
        [InputConnectorDesign("30ecd7a2-0472-4eeb-9698-07f4837790df")]
        public IRuntimeConnector<double> Divisor { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("c45693f9-d872-43bd-80a0-25e7ee98e5dc")]
        public IRuntimeConnector<double> Quotient { get; internal set; }
    }
}

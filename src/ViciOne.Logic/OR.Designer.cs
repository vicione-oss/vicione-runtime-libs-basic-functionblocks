using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Logic;

[FunctionBlockDesign("c659b69e-eb77-40c1-a2be-5cc3ed429ad9", "dc366288-66ee-458f-b71b-db532112148a", "b3540d7b-3635-4948-ad04-4b69d73ec150")]
public partial class OR : RuntimeFunctionBlock<OR>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("4af0b245-cb70-4383-a25e-7d9a790bc68c")]
        public IRuntimeConnector<bool> A { get; internal set; }
        [InputConnectorDesign("ac259e78-86bc-4695-a29e-3605ca18f2b3")]
        public IRuntimeConnector<bool> B { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("5218e0fb-5494-4150-a16c-be1440f29d7f")]
        public IRuntimeConnector<bool> Y { get; internal set; }
    }
}

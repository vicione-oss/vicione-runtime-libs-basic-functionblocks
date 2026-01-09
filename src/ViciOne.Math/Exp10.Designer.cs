using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Math;

[FunctionBlockDesign("7a933dd7-02f8-46f6-b251-d1390d4488c3", "6fd21dba-77cb-4b70-841e-eb888fbcf318", "16f9bc3e-da27-4cf4-97ef-4506d7c20fa4")]
public partial class Exp10 : RuntimeFunctionBlock<Exp10>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("fd6b3340-c76b-4497-8a2e-c287a523e59d")]
        public IRuntimeConnector<double> X { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("0bdd4013-efe8-4ad4-8acc-98316222e01d")]
        public IRuntimeConnector<double> Exp10 { get; internal set; }
    }
}

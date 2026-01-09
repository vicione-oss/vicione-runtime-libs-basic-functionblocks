using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Converter;

[FunctionBlockDesign("13fda31c-3e47-4637-815f-d2bb79196977", "d4298edd-b480-413d-843b-11bba1e540eb", "bc66c05f-b065-4135-9a34-15c1d37841fb")]
public partial class BooleanToDouble : RuntimeFunctionBlock<BooleanToDouble>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("7bbfff36-e8bd-4020-b9a0-a87c2a721144")]
        public IRuntimeConnector<bool> Value { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("4c0a0a44-78d1-40ac-845f-14dc86821637")]
        public IRuntimeConnector<double> Value { get; internal set; }
    }
}

using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Math;

[FunctionBlockDesign("222c0133-61d3-47da-843a-ef1534cce21d", "fabc679c-ffba-411f-ba4d-4037b9a244d6", "52c5d216-60c4-4ed0-9aab-c0d9b00438fc")]
public partial class Floor : RuntimeFunctionBlock<Floor>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("4ca46bb2-8566-4603-99b0-5bcb7a4f6519")]
        public IRuntimeConnector<double> Value { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("cf271923-0f0a-4cb7-8604-2f75ab4b36ad")]
        public IRuntimeConnector<double> Floor { get; internal set; }
    }
}

using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Math;

[FunctionBlockDesign("5ecc471f-9844-467b-8fde-d349c48021f6", "801bfed0-c0d7-4791-9707-33e26a0b795c", "d48e10e4-d0e0-4a38-be10-81deb8c3abd6")]
public partial class Square : RuntimeFunctionBlock<Square>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("f9243caf-1433-4f1e-8584-d48b8c782ed5")]
        public IRuntimeConnector<double> X { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("c6668cd7-8a10-4d75-837e-a3c3ce252f23")]
        public IRuntimeConnector<double> Square { get; internal set; }
    }
}

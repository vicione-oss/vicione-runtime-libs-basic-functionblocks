using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Math;

[FunctionBlockDesign("2a4b70b7-d28c-48d0-bb64-c3985c1009ca", "ffb71385-8a95-4132-9784-e06b694c39e7", "9715025e-c70c-4955-b0ba-ffbbec7c1b44")]
public partial class Power : RuntimeFunctionBlock<Power>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("63685722-d1f7-4e5c-bba2-1aa8510b3079")]
        public IRuntimeConnector<double> X { get; internal set; }
        [InputConnectorDesign("453705e4-6ef9-4473-ade6-f491cf20b75d")]
        public IRuntimeConnector<double> Y { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("0225c934-dbcc-4165-b925-1f8218aa305e")]
        public IRuntimeConnector<double> Power { get; internal set; }
    }
}

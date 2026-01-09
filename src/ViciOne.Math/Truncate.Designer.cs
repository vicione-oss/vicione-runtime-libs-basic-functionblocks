using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Math;

[FunctionBlockDesign("5ca1e785-0fbc-47a1-a8c6-854135fb766d", "6d357de7-2330-4887-9be8-9a68888ad717", "7b8d9bab-e986-4822-a143-d0d55a0d58a9")]
public partial class Truncate : RuntimeFunctionBlock<Truncate>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("9679cd03-1b09-4a7e-a045-8cfe661e6a98")]
        public IRuntimeConnector<double> Value { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("3f98065f-7c68-41fd-bd09-56aa60c73fe5")]
        public IRuntimeConnector<double> Truncate { get; internal set; }
    }
}

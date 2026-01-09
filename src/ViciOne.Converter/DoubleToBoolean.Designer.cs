using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Converter;

[FunctionBlockDesign("c7fba7ca-ea98-41a0-a390-e3b0475c1e31", "ce42cd89-58c1-4f15-9236-f37fc56a90db", "18dc788a-2b91-47e1-9934-3f0087f56c9d")]
public partial class DoubleToBoolean : RuntimeFunctionBlock<DoubleToBoolean>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("cae94711-7036-428c-adad-82b4257cdc6a")]
        public IRuntimeConnector<double> Value { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("49eff823-a449-4e18-96d9-f614c91259aa")]
        public IRuntimeConnector<bool> Value { get; internal set; }
    }
}

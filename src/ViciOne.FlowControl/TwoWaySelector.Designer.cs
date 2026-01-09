using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.FlowControl;

[FunctionBlockDesign("c426099a-9bc3-4234-b182-794a2586bcb0", "0f2917e2-d458-4884-9842-b733db1251eb", "97dd9a15-12e1-4a72-ab08-118ec5737119")]
public partial class TwoWaySelector : RuntimeFunctionBlock<TwoWaySelector>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("c86c2989-7c92-4aa1-9e33-32346a73d57e")]
        public IRuntimeConnector<bool> ManSwitch { get; internal set; }
        [InputConnectorDesign("570c498c-cc6e-415d-8179-972020be041c")]
        public IRuntimeConnector<bool> BinaryAuto { get; internal set; }
        [InputConnectorDesign("54c3a0d6-c1f8-42e3-bec5-4ca8ca6750a3")]
        public IRuntimeConnector<bool> BinaryManual { get; internal set; }
        [InputConnectorDesign("3f6246ae-176f-4354-a026-5586b7a0daea")]
        public IRuntimeConnector<long> LongAuto { get; internal set; }
        [InputConnectorDesign("1e3adbe4-809d-456a-adb0-cd1ab5e9a37c")]
        public IRuntimeConnector<long> LongManual { get; internal set; }
        [InputConnectorDesign("01720f11-d352-446d-a51c-a86fdd11f117")]
        public IRuntimeConnector<double> DoubleAuto { get; internal set; }
        [InputConnectorDesign("36868ca7-8022-4602-8e40-6c5ca11b12b6")]
        public IRuntimeConnector<double> DoubleManual { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("a4126160-d797-4166-9c54-df8d4b2d65d4")]
        public IRuntimeConnector<bool> BinaryOutput { get; internal set; }
        [OutputConnectorDesign("dffb1785-ecff-4de6-b4ae-bd00e7528916")]
        public IRuntimeConnector<long> LongOutput { get; internal set; }
        [OutputConnectorDesign("67cbdfc8-ff23-4c40-a802-d3216a7c4ae8")]
        public IRuntimeConnector<double> DoubleOutput { get; internal set; }
    }
}

using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Math;

[FunctionBlockDesign("fbaf485c-deaa-4f72-9eed-abf330ca8259", "2c012fd8-e129-460c-bd1b-869791f1fd11", "7d034c3d-5fc5-46cb-9d59-066ff63d5fa4")]
public partial class Log10 : RuntimeFunctionBlock<Log10>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("708b3f5e-1549-4b3e-b4fa-abad9a92522e")]
        public IRuntimeConnector<double> X { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("8dbd3b2b-5c8b-4ba2-b45f-76cee31c6b69")]
        public IRuntimeConnector<double> Log10 { get; internal set; }
    }
}

using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Logic;

public partial class XNOR
{
    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
    {
        if (FunctionBlockEnabled)
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Y, ProcessDataInputs.A.Value == ProcessDataInputs.B.Value);
    }
}

using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Logic;

public partial class NOT
{
    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
    {
        if (FunctionBlockEnabled)
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Y, ProcessDataInputs.A.Value is false);
    }
}

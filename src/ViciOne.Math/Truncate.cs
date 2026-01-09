using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Math;

public partial class Truncate
{
    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
    {
        if (FunctionBlockEnabled)
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Truncate, System.Math.Truncate(ProcessDataInputs.Value.Value));
    }
}

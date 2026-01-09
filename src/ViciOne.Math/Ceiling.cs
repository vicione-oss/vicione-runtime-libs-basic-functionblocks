using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Math;

public partial class Ceiling
{
    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
    {
        if (FunctionBlockEnabled)
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Ceiling, System.Math.Ceiling(ProcessDataInputs.Value.Value));
    }
}

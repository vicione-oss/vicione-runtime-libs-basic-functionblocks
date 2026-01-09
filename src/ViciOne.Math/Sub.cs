using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Math;

public partial class Sub
{
    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
    {
        if (FunctionBlockEnabled)
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Difference, ProcessDataInputs.Minuend.Value - ProcessDataInputs.Subtrahend.Value);
    }
}

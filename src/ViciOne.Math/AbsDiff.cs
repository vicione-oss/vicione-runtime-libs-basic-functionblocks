using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Math;

public partial class AbsDiff
{
    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
    {
        if (FunctionBlockEnabled is false)
            return;

        double diff = System.Math.Abs(ProcessDataInputs.Ax.Value - ProcessDataInputs.Ay.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.Diff, diff);
    }
}

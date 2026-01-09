using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Math;

public partial class Power
{
    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
    {
        if (FunctionBlockEnabled)
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Power, System.Math.Pow(ProcessDataInputs.X.Value, ProcessDataInputs.Y.Value));
    }
}

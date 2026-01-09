using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Math;

public partial class Square
{
    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
    {
        if (FunctionBlockEnabled)
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Square, System.Math.Pow(ProcessDataInputs.X.Value, 2));
    }
}

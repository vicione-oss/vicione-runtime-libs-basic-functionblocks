using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Math;

public partial class Exp10
{
    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
    {
        if (FunctionBlockEnabled)
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Exp10, System.Math.Pow(10.0, ProcessDataInputs.X.Value));
    }
}

using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Math;

public partial class Exp
{
    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
    {
        if (FunctionBlockEnabled)
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Exp, System.Math.Exp(ProcessDataInputs.X.Value));
    }
}

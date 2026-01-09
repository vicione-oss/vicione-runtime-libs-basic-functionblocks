using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Math;

public partial class Log10
{
    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
    {
        if (FunctionBlockEnabled && ProcessDataInputs.X.Value > 0)
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Log10, System.Math.Log10(ProcessDataInputs.X.Value));
    }
}

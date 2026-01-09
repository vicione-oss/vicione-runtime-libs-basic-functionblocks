using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Math;

public partial class Log
{
    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
    {
        if (FunctionBlockEnabled && ProcessDataInputs.X.Value > 0)
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Log, System.Math.Log(ProcessDataInputs.X.Value));
    }
}

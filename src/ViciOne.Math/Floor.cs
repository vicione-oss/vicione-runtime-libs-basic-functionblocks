using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Math;

public partial class Floor
{
    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
    {
        if (FunctionBlockEnabled)
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Floor, System.Math.Floor(ProcessDataInputs.Value.Value));
    }
}

using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Math;

public partial class Add
{
    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
    {
        if (FunctionBlockEnabled)
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Sum, ProcessDataInputs.Addend1.Value + ProcessDataInputs.Addend2.Value);
    }
}

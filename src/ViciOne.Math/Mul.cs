using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Math;

public partial class Mul
{
    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
    {
        if (FunctionBlockEnabled)
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Product, ProcessDataInputs.Factor1.Value * ProcessDataInputs.Factor2.Value);
    }
}

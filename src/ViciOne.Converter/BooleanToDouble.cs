using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Converter;

public partial class BooleanToDouble
{
    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
    {
        if (FunctionBlockEnabled)
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Value, ProcessDataInputs.Value.Value ? 1.0 : 0.0);
    }
}

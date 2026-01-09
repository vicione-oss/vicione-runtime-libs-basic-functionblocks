using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Converter;

public partial class LongToDouble
{
    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
    {
        if (FunctionBlockEnabled)
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Value, ProcessDataInputs.Value.Value);
    }
}

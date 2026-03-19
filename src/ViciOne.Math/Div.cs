using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Math;

public partial class Div
{
    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
    {
        if (FunctionBlockEnabled is false)
            return;

        double quotient = 0.0;
        if (ProcessDataInputs.Divisor.Value != 0.0)
            quotient = ProcessDataInputs.Dividend.Value / ProcessDataInputs.Divisor.Value;
        SetConnectorValueIfNotEqual(ProcessDataOutputs.Quotient, quotient);
    }
}

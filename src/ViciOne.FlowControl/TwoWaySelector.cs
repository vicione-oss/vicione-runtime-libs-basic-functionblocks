using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.FlowControl;

public partial class TwoWaySelector
{
    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
    {
        if (FunctionBlockEnabled is false)
            return;

        (bool binaryOutput, long longOutput, double doubleOutput) = SelectOutputValues();

        SetConnectorValueIfNotEqual(ProcessDataOutputs.BinaryOutput, binaryOutput);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.LongOutput, longOutput);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.DoubleOutput, doubleOutput);
    }

    private (bool, long, double) SelectOutputValues()
        => ProcessDataInputs.ManSwitch.Value
            ? (
                    ProcessDataInputs.BinaryManual.Value,
                    ProcessDataInputs.LongManual.Value,
                    ProcessDataInputs.DoubleManual.Value
              )
            : (
                    ProcessDataInputs.BinaryAuto.Value,
                    ProcessDataInputs.LongAuto.Value,
                    ProcessDataInputs.DoubleAuto.Value
              );
}

using AwesomeAssertions;
using ViciOne.FlowControl;
using Xunit;

public class ProcessCycle : RuntimeFunctionBlockTests<TwoWaySelector>
{
    private bool BinaryAutoInputValue { get; } = true;
    private bool BinaryManualInputValue { get; }
    private long LongAutoInputValue { get; } = 42;
    private long LongManualInputValue { get; } = 666;
    private double DoubleAutoInputValue { get; } = 42;
    private double DoubleManualInputValue { get; } = 666;

    [Fact]
    public void DoesNothingIfBlockIsNotEnabled()
    {
        var (binaryOutputBefore, longOutputBefore, doubleOutputBefore) = GetOutputValues();
        SetupInputs(true);
        ChangeEnabled(false);

        ProcessCycle();

        var (binaryOutputAfter, longOutputAfter, doubleOutputAfter) = GetOutputValues();
        binaryOutputAfter.Should().Be(binaryOutputBefore);
        longOutputAfter.Should().Be(longOutputBefore);
        doubleOutputAfter.Should().Be(doubleOutputBefore);
    }

    [Fact]
    public void PassesAutoInputValueWhenManSwitchInputIsFalse()
    {
        SetupInputs(false);

        ProcessCycle();

        var (binaryOutput, longOutput, doubleOutput) = GetOutputValues();
        binaryOutput.Should().Be(BinaryAutoInputValue);
        longOutput.Should().Be(LongAutoInputValue);
        doubleOutput.Should().Be(DoubleAutoInputValue);
    }

    [Fact]
    public void PassesManualInputValueWhenManSwitchInputIsTrue()
    {
        SetupInputs(true);

        ProcessCycle();

        var (binaryOutput, longOutput, doubleOutput) = GetOutputValues();
        binaryOutput.Should().Be(BinaryManualInputValue);
        longOutput.Should().Be(LongManualInputValue);
        doubleOutput.Should().Be(DoubleManualInputValue);
    }

    private void SetupInputs(bool manSwitchValue)
    {
        ChangeInputValue(nameof(TwoWaySelector.ProcessDataInputs.ManSwitch), manSwitchValue);
        ChangeInputValue(nameof(TwoWaySelector.ProcessDataInputs.BinaryAuto), BinaryAutoInputValue);
        ChangeInputValue(nameof(TwoWaySelector.ProcessDataInputs.BinaryManual), BinaryManualInputValue);
        ChangeInputValue(nameof(TwoWaySelector.ProcessDataInputs.LongAuto), LongAutoInputValue);
        ChangeInputValue(nameof(TwoWaySelector.ProcessDataInputs.LongManual), LongManualInputValue);
        ChangeInputValue(nameof(TwoWaySelector.ProcessDataInputs.DoubleAuto), DoubleAutoInputValue);
        ChangeInputValue(nameof(TwoWaySelector.ProcessDataInputs.DoubleManual), DoubleManualInputValue);
    }

    private (bool BinaryOutput, long LongOutput, double DoubleOutput) GetOutputValues()
        => (
            GetOutputValue<bool>(nameof(TwoWaySelector.ProcessDataOutputs.BinaryOutput)),
            GetOutputValue<long>(nameof(TwoWaySelector.ProcessDataOutputs.LongOutput)),
            GetOutputValue<double>(nameof(TwoWaySelector.ProcessDataOutputs.DoubleOutput)));
}

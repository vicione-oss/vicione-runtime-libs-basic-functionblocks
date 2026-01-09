using System;
using Xunit;
using AwesomeAssertions;

namespace ViciOne.FlowControl.Tests;

public class ValueRepeaterTests : RuntimeFunctionBlockTests<ValueRepeater>
{
    [Fact]
    public void DoesNothingIfBlockIsNotEnabled()
    {
        // Arrange
        ChangeEnabled(false);

        // Act
        var outputValueBefore = GetOutputValue<double>(nameof(ValueRepeater.ProcessDataOutputs.DoubleValue));
        ProcessCycle();

        // Assert
        var outputValueAfter = GetOutputValue<double>(nameof(ValueRepeater.ProcessDataOutputs.DoubleValue));
        outputValueAfter.Should().Be(outputValueBefore);
    }

    [Theory]
    [InlineData(12.4)]
    [InlineData(-10.5)]
    [InlineData(0.0)]
    public void RepeatsDoubleValue(double inputValue)
    {
        // Arrange
        ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.DoubleValue), inputValue);

        // Act
        ProcessCycle();

        // Assert
        var outputValue = GetOutputValue<double>(nameof(ValueRepeater.ProcessDataOutputs.DoubleValue));
        outputValue.Should().Be(inputValue);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void RepeatsBoolValue(bool inputValue)
    {
        // Arrange
        ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.BoolValue), inputValue);

        // Act
        ProcessCycle();

        // Assert
        var outputValue = GetOutputValue<bool>(nameof(ValueRepeater.ProcessDataOutputs.BoolValue));
        outputValue.Should().Be(inputValue);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(42)]
    [InlineData(-100)]
    public void RepeatsIntValue(int inputValue)
    {
        // Arrange
        ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.IntValue), inputValue);

        // Act
        ProcessCycle();

        // Assert
        var outputValue = GetOutputValue<int>(nameof(ValueRepeater.ProcessDataOutputs.IntValue));
        outputValue.Should().Be(inputValue);
    }

    [Theory]
    [InlineData("Hello")]
    [InlineData("")]
    [InlineData("Test String 123")]
    public void RepeatsStringValue(string inputValue)
    {
        // Arrange
        ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.StringValue), inputValue);

        // Act
        ProcessCycle();

        // Assert
        var outputValue = GetOutputValue<string>(nameof(ValueRepeater.ProcessDataOutputs.StringValue));
        outputValue.Should().Be(inputValue);
    }

    [Fact]
    public void RepeatsChangedDoubleValue()
    {
        // Arrange
        var inputValue1 = 10.5;
        var inputValue2 = 20.5;

        // Act
        ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.DoubleValue), inputValue1);
        ProcessCycle();
        var outputValue1 = GetOutputValue<double>(nameof(ValueRepeater.ProcessDataOutputs.DoubleValue));

        ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.DoubleValue), inputValue2);
        ProcessCycle();
        var outputValue2 = GetOutputValue<double>(nameof(ValueRepeater.ProcessDataOutputs.DoubleValue));

        // Assert
        outputValue1.Should().Be(inputValue1);
        outputValue2.Should().Be(inputValue2);
    }

    [Fact]
    public void RepeatsMultipleValuesSimultaneously()
    {
        // Arrange
        var doubleInputValue = 42.5;
        var boolInputValue = true;
        var intInputValue = 123;
        var stringInputValue = "Test";

        ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.DoubleValue), doubleInputValue);
        ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.BoolValue), boolInputValue);
        ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.IntValue), intInputValue);
        ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.StringValue), stringInputValue);

        // Act
        ProcessCycle();
        var doubleOutputValue = GetOutputValue<double>(nameof(ValueRepeater.ProcessDataOutputs.DoubleValue));
        var boolOutputValue = GetOutputValue<bool>(nameof(ValueRepeater.ProcessDataOutputs.BoolValue));
        var intOutputValue = GetOutputValue<int>(nameof(ValueRepeater.ProcessDataOutputs.IntValue));
        var stringOutputValue = GetOutputValue<string>(nameof(ValueRepeater.ProcessDataOutputs.StringValue));

        // Assert
        doubleOutputValue.Should().Be(doubleInputValue);
        boolOutputValue.Should().Be(boolInputValue);
        intOutputValue.Should().Be(intInputValue);
        stringOutputValue.Should().Be(stringInputValue);
    }

    [Fact]
    public void CalculatesCorrectIfBlockWasDisabled()
    {
        // Arrange
        var inputValue = 50.0;
        ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.DoubleValue), inputValue);
        ChangeEnabled(false);

        // Act
        ProcessCycle();
        ChangeEnabled(true);
        ProcessCycle();

        // Assert
        var outputValueAfter = GetOutputValue<double>(nameof(ValueRepeater.ProcessDataOutputs.DoubleValue));
        outputValueAfter.Should().Be(inputValue);
    }

    [Theory]
    [InlineData((byte)255)]
    [InlineData((byte)0)]
    [InlineData((byte)128)]
    public void RepeatsByteValue(byte inputValue)
    {
        // Arrange
        ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.ByteValue), inputValue);

        // Act
        ProcessCycle();

        // Assert
        var outputValue = GetOutputValue<byte>(nameof(ValueRepeater.ProcessDataOutputs.ByteValue));
        outputValue.Should().Be(inputValue);
    }

    [Theory]
    [InlineData((short)100)]
    [InlineData((short)-100)]
    public void RepeatsShortValue(short inputValue)
    {
        // Arrange
        ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.ShortValue), inputValue);

        // Act
        ProcessCycle();

        // Assert
        var outputValue = GetOutputValue<short>(nameof(ValueRepeater.ProcessDataOutputs.ShortValue));
        outputValue.Should().Be(inputValue);
    }

    [Theory]
    [InlineData(1000L)]
    [InlineData(-1000L)]
    public void RepeatsLongValue(long inputValue)
    {
        // Arrange
        ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.LongValue), inputValue);

        // Act
        ProcessCycle();

        // Assert
        var outputValue = GetOutputValue<long>(nameof(ValueRepeater.ProcessDataOutputs.LongValue));
        outputValue.Should().Be(inputValue);
    }

    [Theory]
    [InlineData(3.14f)]
    [InlineData(-2.5f)]
    public void RepeatsFloatValue(float inputValue)
    {
        // Arrange
        ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.FloatValue), inputValue);

        // Act
        ProcessCycle();

        // Assert
        var outputValue = GetOutputValue<float>(nameof(ValueRepeater.ProcessDataOutputs.FloatValue));
        outputValue.Should().Be(inputValue);
    }

    [Fact]
    public void RepeatsDateTimeValue()
    {
        // Arrange
        var inputValue = new DateTime(2025, 12, 5, 14, 30, 45);
        ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.DateTimeValue), inputValue);

        // Act
        ProcessCycle();

        // Assert
        var outputValue = GetOutputValue<DateTime>(nameof(ValueRepeater.ProcessDataOutputs.DateTimeValue));
        outputValue.Should().Be(inputValue);
    }

    [Fact]
    public void RepeatsTimeSpanValue()
    {
        // Arrange
        var inputValue = TimeSpan.FromHours(5.5);
        ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.TimeSpanValue), inputValue);

        // Act
        ProcessCycle();

        // Assert
        var outputValue = GetOutputValue<TimeSpan>(nameof(ValueRepeater.ProcessDataOutputs.TimeSpanValue));
        outputValue.Should().Be(inputValue);
    }

    [Fact]
    public void RepeatsChangedDateTimeValue()
    {
        // Arrange
        var inputValue1 = new DateTime(2025, 1, 1, 0, 0, 0);
        var inputValue2 = new DateTime(2025, 12, 31, 23, 59, 59);

        // Act
        ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.DateTimeValue), inputValue1);
        ProcessCycle();
        var outputValue1 = GetOutputValue<DateTime>(nameof(ValueRepeater.ProcessDataOutputs.DateTimeValue));

        ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.DateTimeValue), inputValue2);
        ProcessCycle();
        var outputValue2 = GetOutputValue<DateTime>(nameof(ValueRepeater.ProcessDataOutputs.DateTimeValue));

        // Assert
        outputValue1.Should().Be(inputValue1);
        outputValue2.Should().Be(inputValue2);
    }

    [Fact]
    public void RepeatsChangedTimeSpanValue()
    {
        // Arrange
        var inputValue1 = TimeSpan.FromMinutes(30);
        var inputValue2 = TimeSpan.FromHours(2);

        // Act
        ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.TimeSpanValue), inputValue1);
        ProcessCycle();
        var outputValue1 = GetOutputValue<TimeSpan>(nameof(ValueRepeater.ProcessDataOutputs.TimeSpanValue));

        ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.TimeSpanValue), inputValue2);
        ProcessCycle();
        var outputValue2 = GetOutputValue<TimeSpan>(nameof(ValueRepeater.ProcessDataOutputs.TimeSpanValue));

        // Assert
        outputValue1.Should().Be(inputValue1);
        outputValue2.Should().Be(inputValue2);
    }
}

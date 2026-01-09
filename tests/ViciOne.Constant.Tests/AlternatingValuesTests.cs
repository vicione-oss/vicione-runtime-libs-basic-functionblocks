using System;
using AwesomeAssertions;
using Xunit;

namespace ViciOne.Constant.Tests;

public class AlternatingValuesTests : RuntimeFunctionBlockTests<AlternatingValues>
{
    // Base test-class calls process cycle in constructor, so we need to set initial state
    public AlternatingValuesTests() => GetRuntimeFunctionBlock().UseValueOne = true;

    [Fact]
    public void DoesNothingIfBlockIsNotEnabled()
    {
        // Arrange
        ChangeEnabled(false);

        // Act
        var outputValueBefore = GetOutputValue<double>(nameof(AlternatingValues.ProcessDataOutputs.DoubleValue));
        ProcessCycle();

        // Assert
        var outputValueAfter = GetOutputValue<double>(nameof(AlternatingValues.ProcessDataOutputs.DoubleValue));
        outputValueAfter.Should().Be(outputValueBefore);
    }

    [Fact]
    public void AlternatesBetweenDoubleValues()
    {
        // Arrange
        var valueOne = 10.5;
        var valueTwo = 20.5;
        ChangeSettingValue(nameof(AlternatingValues.Settings.DoubleValueOne), valueOne);
        ChangeSettingValue(nameof(AlternatingValues.Settings.DoubleValueTwo), valueTwo);

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<double>(nameof(AlternatingValues.ProcessDataOutputs.DoubleValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<double>(nameof(AlternatingValues.ProcessDataOutputs.DoubleValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<double>(nameof(AlternatingValues.ProcessDataOutputs.DoubleValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenDoubleDefaultValues()
    {
        // Arrange
        var valueOne = 100.0;
        var valueTwo = 200.0;

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<double>(nameof(AlternatingValues.ProcessDataOutputs.DoubleValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<double>(nameof(AlternatingValues.ProcessDataOutputs.DoubleValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<double>(nameof(AlternatingValues.ProcessDataOutputs.DoubleValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenBoolValues()
    {
        // Arrange
        var valueOne = false;
        var valueTwo = true;
        ChangeSettingValue(nameof(AlternatingValues.Settings.BoolValueOne), valueOne);
        ChangeSettingValue(nameof(AlternatingValues.Settings.BoolValueTwo), valueTwo);

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<bool>(nameof(AlternatingValues.ProcessDataOutputs.BoolValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<bool>(nameof(AlternatingValues.ProcessDataOutputs.BoolValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<bool>(nameof(AlternatingValues.ProcessDataOutputs.BoolValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenBoolDefaultValues()
    {
        // Arrange
        var valueOne = true;
        var valueTwo = false;

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<bool>(nameof(AlternatingValues.ProcessDataOutputs.BoolValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<bool>(nameof(AlternatingValues.ProcessDataOutputs.BoolValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<bool>(nameof(AlternatingValues.ProcessDataOutputs.BoolValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenIntValues()
    {
        // Arrange
        var valueOne = 42;
        var valueTwo = 84;
        ChangeSettingValue(nameof(AlternatingValues.Settings.IntValueOne), valueOne);
        ChangeSettingValue(nameof(AlternatingValues.Settings.IntValueTwo), valueTwo);

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<int>(nameof(AlternatingValues.ProcessDataOutputs.IntValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<int>(nameof(AlternatingValues.ProcessDataOutputs.IntValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<int>(nameof(AlternatingValues.ProcessDataOutputs.IntValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenIntDefaultValues()
    {
        // Arrange
        var valueOne = 50000;
        var valueTwo = 100000;

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<int>(nameof(AlternatingValues.ProcessDataOutputs.IntValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<int>(nameof(AlternatingValues.ProcessDataOutputs.IntValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<int>(nameof(AlternatingValues.ProcessDataOutputs.IntValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenStringValues()
    {
        // Arrange
        var valueOne = "Foo";
        var valueTwo = "Bar";
        ChangeSettingValue(nameof(AlternatingValues.Settings.StringValueOne), valueOne);
        ChangeSettingValue(nameof(AlternatingValues.Settings.StringValueTwo), valueTwo);

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<string>(nameof(AlternatingValues.ProcessDataOutputs.StringValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<string>(nameof(AlternatingValues.ProcessDataOutputs.StringValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<string>(nameof(AlternatingValues.ProcessDataOutputs.StringValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenStringDefaultValues()
    {
        // Arrange
        var valueOne = "Hello";
        var valueTwo = "World";

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<string>(nameof(AlternatingValues.ProcessDataOutputs.StringValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<string>(nameof(AlternatingValues.ProcessDataOutputs.StringValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<string>(nameof(AlternatingValues.ProcessDataOutputs.StringValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenByteValues()
    {
        // Arrange
        var valueOne = (byte)50;
        var valueTwo = (byte)100;
        ChangeSettingValue(nameof(AlternatingValues.Settings.ByteValueOne), valueOne);
        ChangeSettingValue(nameof(AlternatingValues.Settings.ByteValueTwo), valueTwo);

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<byte>(nameof(AlternatingValues.ProcessDataOutputs.ByteValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<byte>(nameof(AlternatingValues.ProcessDataOutputs.ByteValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<byte>(nameof(AlternatingValues.ProcessDataOutputs.ByteValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenByteDefaultValues()
    {
        // Arrange
        var valueOne = (byte)10;
        var valueTwo = (byte)20;

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<byte>(nameof(AlternatingValues.ProcessDataOutputs.ByteValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<byte>(nameof(AlternatingValues.ProcessDataOutputs.ByteValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<byte>(nameof(AlternatingValues.ProcessDataOutputs.ByteValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenSByteValues()
    {
        // Arrange
        var valueOne = (sbyte)-10;
        var valueTwo = (sbyte)15;
        ChangeSettingValue(nameof(AlternatingValues.Settings.SByteValueOne), valueOne);
        ChangeSettingValue(nameof(AlternatingValues.Settings.SByteValueTwo), valueTwo);

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<sbyte>(nameof(AlternatingValues.ProcessDataOutputs.SByteValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<sbyte>(nameof(AlternatingValues.ProcessDataOutputs.SByteValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<sbyte>(nameof(AlternatingValues.ProcessDataOutputs.SByteValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenSByteDefaultValues()
    {
        // Arrange
        var valueOne = (sbyte)-5;
        var valueTwo = (sbyte)5;

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<sbyte>(nameof(AlternatingValues.ProcessDataOutputs.SByteValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<sbyte>(nameof(AlternatingValues.ProcessDataOutputs.SByteValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<sbyte>(nameof(AlternatingValues.ProcessDataOutputs.SByteValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenCharValues()
    {
        // Arrange
        var valueOne = 'X';
        var valueTwo = 'Y';
        ChangeSettingValue(nameof(AlternatingValues.Settings.CharValueOne), valueOne);
        ChangeSettingValue(nameof(AlternatingValues.Settings.CharValueTwo), valueTwo);

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<char>(nameof(AlternatingValues.ProcessDataOutputs.CharValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<char>(nameof(AlternatingValues.ProcessDataOutputs.CharValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<char>(nameof(AlternatingValues.ProcessDataOutputs.CharValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenCharDefaultValues()
    {
        // Arrange
        var valueOne = 'A';
        var valueTwo = 'B';

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<char>(nameof(AlternatingValues.ProcessDataOutputs.CharValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<char>(nameof(AlternatingValues.ProcessDataOutputs.CharValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<char>(nameof(AlternatingValues.ProcessDataOutputs.CharValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenShortValues()
    {
        // Arrange
        var valueOne = (short)500;
        var valueTwo = (short)750;
        ChangeSettingValue(nameof(AlternatingValues.Settings.ShortValueOne), valueOne);
        ChangeSettingValue(nameof(AlternatingValues.Settings.ShortValueTwo), valueTwo);

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<short>(nameof(AlternatingValues.ProcessDataOutputs.ShortValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<short>(nameof(AlternatingValues.ProcessDataOutputs.ShortValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<short>(nameof(AlternatingValues.ProcessDataOutputs.ShortValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenShortDefaultValues()
    {
        // Arrange
        var valueOne = (short)1000;
        var valueTwo = (short)2000;

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<short>(nameof(AlternatingValues.ProcessDataOutputs.ShortValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<short>(nameof(AlternatingValues.ProcessDataOutputs.ShortValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<short>(nameof(AlternatingValues.ProcessDataOutputs.ShortValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenUShortValues()
    {
        // Arrange
        var valueOne = (ushort)5000;
        var valueTwo = (ushort)6000;
        ChangeSettingValue(nameof(AlternatingValues.Settings.UShortValueOne), valueOne);
        ChangeSettingValue(nameof(AlternatingValues.Settings.UShortValueTwo), valueTwo);

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<ushort>(nameof(AlternatingValues.ProcessDataOutputs.UShortValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<ushort>(nameof(AlternatingValues.ProcessDataOutputs.UShortValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<ushort>(nameof(AlternatingValues.ProcessDataOutputs.UShortValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenUShortDefaultValues()
    {
        // Arrange
        var valueOne = (ushort)3000;
        var valueTwo = (ushort)4000;

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<ushort>(nameof(AlternatingValues.ProcessDataOutputs.UShortValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<ushort>(nameof(AlternatingValues.ProcessDataOutputs.UShortValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<ushort>(nameof(AlternatingValues.ProcessDataOutputs.UShortValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenUIntValues()
    {
        // Arrange
        var valueOne = 1000000000u;
        var valueTwo = 2000000000u;
        ChangeSettingValue(nameof(AlternatingValues.Settings.UIntValueOne), valueOne);
        ChangeSettingValue(nameof(AlternatingValues.Settings.UIntValueTwo), valueTwo);

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<uint>(nameof(AlternatingValues.ProcessDataOutputs.UIntValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<uint>(nameof(AlternatingValues.ProcessDataOutputs.UIntValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<uint>(nameof(AlternatingValues.ProcessDataOutputs.UIntValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenUIntDefaultValues()
    {
        // Arrange
        var valueOne = 3000000000u;
        var valueTwo = 4000000000u;

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<uint>(nameof(AlternatingValues.ProcessDataOutputs.UIntValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<uint>(nameof(AlternatingValues.ProcessDataOutputs.UIntValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<uint>(nameof(AlternatingValues.ProcessDataOutputs.UIntValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenLongValues()
    {
        // Arrange
        var valueOne = 5000000000L;
        var valueTwo = 6000000000L;
        ChangeSettingValue(nameof(AlternatingValues.Settings.LongValueOne), valueOne);
        ChangeSettingValue(nameof(AlternatingValues.Settings.LongValueTwo), valueTwo);

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<long>(nameof(AlternatingValues.ProcessDataOutputs.LongValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<long>(nameof(AlternatingValues.ProcessDataOutputs.LongValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<long>(nameof(AlternatingValues.ProcessDataOutputs.LongValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenLongDefaultValues()
    {
        // Arrange
        var valueOne = 9000000000L;
        var valueTwo = 9500000000L;

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<long>(nameof(AlternatingValues.ProcessDataOutputs.LongValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<long>(nameof(AlternatingValues.ProcessDataOutputs.LongValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<long>(nameof(AlternatingValues.ProcessDataOutputs.LongValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenULongValues()
    {
        // Arrange
        var valueOne = 10000000000UL;
        var valueTwo = 12000000000UL;
        ChangeSettingValue(nameof(AlternatingValues.Settings.ULongValueOne), valueOne);
        ChangeSettingValue(nameof(AlternatingValues.Settings.ULongValueTwo), valueTwo);

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<ulong>(nameof(AlternatingValues.ProcessDataOutputs.ULongValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<ulong>(nameof(AlternatingValues.ProcessDataOutputs.ULongValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<ulong>(nameof(AlternatingValues.ProcessDataOutputs.ULongValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenULongDefaultValues()
    {
        // Arrange
        var valueOne = 18000000000UL;
        var valueTwo = 18500000000UL;

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<ulong>(nameof(AlternatingValues.ProcessDataOutputs.ULongValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<ulong>(nameof(AlternatingValues.ProcessDataOutputs.ULongValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<ulong>(nameof(AlternatingValues.ProcessDataOutputs.ULongValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenFloatValues()
    {
        // Arrange
        var valueOne = 1.23f;
        var valueTwo = 4.56f;
        ChangeSettingValue(nameof(AlternatingValues.Settings.FloatValueOne), valueOne);
        ChangeSettingValue(nameof(AlternatingValues.Settings.FloatValueTwo), valueTwo);

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<float>(nameof(AlternatingValues.ProcessDataOutputs.FloatValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<float>(nameof(AlternatingValues.ProcessDataOutputs.FloatValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<float>(nameof(AlternatingValues.ProcessDataOutputs.FloatValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenFloatDefaultValues()
    {
        // Arrange
        var valueOne = 3.14f;
        var valueTwo = 2.71f;

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<float>(nameof(AlternatingValues.ProcessDataOutputs.FloatValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<float>(nameof(AlternatingValues.ProcessDataOutputs.FloatValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<float>(nameof(AlternatingValues.ProcessDataOutputs.FloatValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenMultipleTypesSimultaneously()
    {
        // Arrange
        var doubleValueOne = 10.5;
        var doubleValueTwo = 20.5;
        var boolValueOne = false;
        var boolValueTwo = true;
        var intValueOne = 111;
        var intValueTwo = 222;

        ChangeSettingValue(nameof(AlternatingValues.Settings.DoubleValueOne), doubleValueOne);
        ChangeSettingValue(nameof(AlternatingValues.Settings.DoubleValueTwo), doubleValueTwo);
        ChangeSettingValue(nameof(AlternatingValues.Settings.BoolValueOne), boolValueOne);
        ChangeSettingValue(nameof(AlternatingValues.Settings.BoolValueTwo), boolValueTwo);
        ChangeSettingValue(nameof(AlternatingValues.Settings.IntValueOne), intValueOne);
        ChangeSettingValue(nameof(AlternatingValues.Settings.IntValueTwo), intValueTwo);

        // Act
        ProcessCycle();
        var doubleOutputValue1 = GetOutputValue<double>(nameof(AlternatingValues.ProcessDataOutputs.DoubleValue));
        var boolOutputValue1 = GetOutputValue<bool>(nameof(AlternatingValues.ProcessDataOutputs.BoolValue));
        var intOutputValue1 = GetOutputValue<int>(nameof(AlternatingValues.ProcessDataOutputs.IntValue));
        ProcessCycle();
        var doubleOutputValue2 = GetOutputValue<double>(nameof(AlternatingValues.ProcessDataOutputs.DoubleValue));
        var boolOutputValue2 = GetOutputValue<bool>(nameof(AlternatingValues.ProcessDataOutputs.BoolValue));
        var intOutputValue2 = GetOutputValue<int>(nameof(AlternatingValues.ProcessDataOutputs.IntValue));
        ProcessCycle();
        var doubleOutputValue3 = GetOutputValue<double>(nameof(AlternatingValues.ProcessDataOutputs.DoubleValue));
        var boolOutputValue3 = GetOutputValue<bool>(nameof(AlternatingValues.ProcessDataOutputs.BoolValue));
        var intOutputValue3 = GetOutputValue<int>(nameof(AlternatingValues.ProcessDataOutputs.IntValue));

        // Assert
        doubleOutputValue1.Should().Be(doubleValueOne);
        boolOutputValue1.Should().Be(boolValueOne);
        intOutputValue1.Should().Be(intValueOne);

        doubleOutputValue2.Should().Be(doubleValueTwo);
        boolOutputValue2.Should().Be(boolValueTwo);
        intOutputValue2.Should().Be(intValueTwo);

        doubleOutputValue3.Should().Be(doubleValueOne);
        boolOutputValue3.Should().Be(boolValueOne);
        intOutputValue3.Should().Be(intValueOne);
    }

    [Fact]
    public void AlternatesBetweenDecimalValues()
    {
        // Arrange
        var valueOne = 123.45m;
        var valueTwo = 678.90m;
        ChangeSettingValue(nameof(AlternatingValues.Settings.DecimalValueOne), valueOne);
        ChangeSettingValue(nameof(AlternatingValues.Settings.DecimalValueTwo), valueTwo);

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<decimal>(nameof(AlternatingValues.ProcessDataOutputs.DecimalValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<decimal>(nameof(AlternatingValues.ProcessDataOutputs.DecimalValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<decimal>(nameof(AlternatingValues.ProcessDataOutputs.DecimalValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenDecimalDefaultValues()
    {
        // Arrange
        // Default Werte aus der Designer-Klasse
        var valueOne = 3.14m;
        var valueTwo = 2.71m;

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<decimal>(nameof(AlternatingValues.ProcessDataOutputs.DecimalValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<decimal>(nameof(AlternatingValues.ProcessDataOutputs.DecimalValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<decimal>(nameof(AlternatingValues.ProcessDataOutputs.DecimalValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenDateTimeValues()
    {
        // Arrange
        var valueOne = new DateTime(2025, 1, 1, 10, 0, 0);
        var valueTwo = new DateTime(2025, 12, 31, 18, 30, 0);
        ChangeSettingValue(nameof(AlternatingValues.Settings.DateTimeValueOne), valueOne);
        ChangeSettingValue(nameof(AlternatingValues.Settings.DateTimeValueTwo), valueTwo);

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<DateTime>(nameof(AlternatingValues.ProcessDataOutputs.DateTimeValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<DateTime>(nameof(AlternatingValues.ProcessDataOutputs.DateTimeValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<DateTime>(nameof(AlternatingValues.ProcessDataOutputs.DateTimeValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenDateTimeDefaultValues()
    {
        // Arrange
        // Default Werte aus der Designer-Klasse: 2025-01-01T10:00:00 and 2025-12-31T18:30:00
        var valueOne = new DateTime(2025, 1, 1, 10, 0, 0);
        var valueTwo = new DateTime(2025, 12, 31, 18, 30, 0);

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<DateTime>(nameof(AlternatingValues.ProcessDataOutputs.DateTimeValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<DateTime>(nameof(AlternatingValues.ProcessDataOutputs.DateTimeValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<DateTime>(nameof(AlternatingValues.ProcessDataOutputs.DateTimeValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenTimeSpanValues()
    {
        // Arrange
        var valueOne = TimeSpan.FromHours(2);
        var valueTwo = TimeSpan.FromMinutes(45);
        ChangeSettingValue(nameof(AlternatingValues.Settings.TimeSpanValueOne), valueOne);
        ChangeSettingValue(nameof(AlternatingValues.Settings.TimeSpanValueTwo), valueTwo);

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<TimeSpan>(nameof(AlternatingValues.ProcessDataOutputs.TimeSpanValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<TimeSpan>(nameof(AlternatingValues.ProcessDataOutputs.TimeSpanValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<TimeSpan>(nameof(AlternatingValues.ProcessDataOutputs.TimeSpanValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }

    [Fact]
    public void AlternatesBetweenTimeSpanDefaultValues()
    {
        // Arrange
        // Default Werte aus der Designer-Klasse: 02:00:00 and 00:45:00
        var valueOne = TimeSpan.FromHours(2);
        var valueTwo = TimeSpan.FromMinutes(45);

        // Act
        ProcessCycle();
        var outputValue1 = GetOutputValue<TimeSpan>(nameof(AlternatingValues.ProcessDataOutputs.TimeSpanValue));
        ProcessCycle();
        var outputValue2 = GetOutputValue<TimeSpan>(nameof(AlternatingValues.ProcessDataOutputs.TimeSpanValue));
        ProcessCycle();
        var outputValue3 = GetOutputValue<TimeSpan>(nameof(AlternatingValues.ProcessDataOutputs.TimeSpanValue));

        // Assert
        outputValue1.Should().Be(valueOne);
        outputValue2.Should().Be(valueTwo);
        outputValue3.Should().Be(valueOne);
    }
}

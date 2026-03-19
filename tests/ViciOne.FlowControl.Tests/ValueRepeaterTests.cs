using System;
using AwesomeAssertions;
using Xunit;

namespace ViciOne.FlowControl.Tests;

public class ValueRepeaterTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<ValueRepeater>
    {
        [Fact]
        public void DoesNothingIfBlockIsNotEnabled()
        {
            ChangeEnabled(false);

            var outputValueBefore = GetOutputValue<double>(nameof(ValueRepeater.ProcessDataOutputs.DoubleValue));
            ProcessCycle();

            var outputValueAfter = GetOutputValue<double>(nameof(ValueRepeater.ProcessDataOutputs.DoubleValue));
            outputValueAfter.Should().Be(outputValueBefore);
        }

        [Theory]
        [InlineData(12.4)]
        [InlineData(-10.5)]
        [InlineData(0.0)]
        public void RepeatsDoubleValue(double inputValue)
        {
            ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.DoubleValue), inputValue);

            ProcessCycle();

            var outputValue = GetOutputValue<double>(nameof(ValueRepeater.ProcessDataOutputs.DoubleValue));
            outputValue.Should().Be(inputValue);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void RepeatsBoolValue(bool inputValue)
        {
            ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.BoolValue), inputValue);

            ProcessCycle();

            var outputValue = GetOutputValue<bool>(nameof(ValueRepeater.ProcessDataOutputs.BoolValue));
            outputValue.Should().Be(inputValue);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(42)]
        [InlineData(-100)]
        public void RepeatsIntValue(int inputValue)
        {
            ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.IntValue), inputValue);

            ProcessCycle();

            var outputValue = GetOutputValue<int>(nameof(ValueRepeater.ProcessDataOutputs.IntValue));
            outputValue.Should().Be(inputValue);
        }

        [Theory]
        [InlineData("Hello")]
        [InlineData("")]
        [InlineData("Test String 123")]
        public void RepeatsStringValue(string inputValue)
        {
            ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.StringValue), inputValue);

            ProcessCycle();

            var outputValue = GetOutputValue<string>(nameof(ValueRepeater.ProcessDataOutputs.StringValue));
            outputValue.Should().Be(inputValue);
        }

        [Fact]
        public void RepeatsChangedDoubleValue()
        {
            var inputValue1 = 10.5;
            var inputValue2 = 20.5;

            ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.DoubleValue), inputValue1);
            ProcessCycle();
            var outputValue1 = GetOutputValue<double>(nameof(ValueRepeater.ProcessDataOutputs.DoubleValue));
            ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.DoubleValue), inputValue2);
            ProcessCycle();
            var outputValue2 = GetOutputValue<double>(nameof(ValueRepeater.ProcessDataOutputs.DoubleValue));

            outputValue1.Should().Be(inputValue1);
            outputValue2.Should().Be(inputValue2);
        }

        [Fact]
        public void RepeatsMultipleValuesSimultaneously()
        {
            var doubleInputValue = 42.5;
            var boolInputValue = true;
            var intInputValue = 123;
            var stringInputValue = "Test";
            ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.DoubleValue), doubleInputValue);
            ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.BoolValue), boolInputValue);
            ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.IntValue), intInputValue);
            ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.StringValue), stringInputValue);

            ProcessCycle();

            var doubleOutputValue = GetOutputValue<double>(nameof(ValueRepeater.ProcessDataOutputs.DoubleValue));
            var boolOutputValue = GetOutputValue<bool>(nameof(ValueRepeater.ProcessDataOutputs.BoolValue));
            var intOutputValue = GetOutputValue<int>(nameof(ValueRepeater.ProcessDataOutputs.IntValue));
            var stringOutputValue = GetOutputValue<string>(nameof(ValueRepeater.ProcessDataOutputs.StringValue));
            doubleOutputValue.Should().Be(doubleInputValue);
            boolOutputValue.Should().Be(boolInputValue);
            intOutputValue.Should().Be(intInputValue);
            stringOutputValue.Should().Be(stringInputValue);
        }

        [Fact]
        public void CalculatesCorrectIfBlockWasDisabled()
        {
            var inputValue = 50.0;
            ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.DoubleValue), inputValue);
            ChangeEnabled(false);
            ProcessCycle();
            ChangeEnabled(true);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<double>(nameof(ValueRepeater.ProcessDataOutputs.DoubleValue));
            outputValueAfter.Should().Be(inputValue);
        }

        [Theory]
        [InlineData((byte)255)]
        [InlineData((byte)0)]
        [InlineData((byte)128)]
        public void RepeatsByteValue(byte inputValue)
        {
            ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.ByteValue), inputValue);

            ProcessCycle();

            var outputValue = GetOutputValue<byte>(nameof(ValueRepeater.ProcessDataOutputs.ByteValue));
            outputValue.Should().Be(inputValue);
        }

        [Theory]
        [InlineData((short)100)]
        [InlineData((short)-100)]
        public void RepeatsShortValue(short inputValue)
        {
            ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.ShortValue), inputValue);

            ProcessCycle();

            var outputValue = GetOutputValue<short>(nameof(ValueRepeater.ProcessDataOutputs.ShortValue));
            outputValue.Should().Be(inputValue);
        }

        [Theory]
        [InlineData(1000L)]
        [InlineData(-1000L)]
        public void RepeatsLongValue(long inputValue)
        {
            ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.LongValue), inputValue);

            ProcessCycle();

            var outputValue = GetOutputValue<long>(nameof(ValueRepeater.ProcessDataOutputs.LongValue));
            outputValue.Should().Be(inputValue);
        }

        [Theory]
        [InlineData(3.14f)]
        [InlineData(-2.5f)]
        public void RepeatsFloatValue(float inputValue)
        {
            ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.FloatValue), inputValue);

            ProcessCycle();

            var outputValue = GetOutputValue<float>(nameof(ValueRepeater.ProcessDataOutputs.FloatValue));
            outputValue.Should().Be(inputValue);
        }

        [Fact]
        public void RepeatsDateTimeValue()
        {
            var inputValue = new DateTime(2025, 12, 5, 14, 30, 45);
            ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.DateTimeValue), inputValue);

            ProcessCycle();

            var outputValue = GetOutputValue<DateTime>(nameof(ValueRepeater.ProcessDataOutputs.DateTimeValue));
            outputValue.Should().Be(inputValue);
        }

        [Fact]
        public void RepeatsTimeSpanValue()
        {
            var inputValue = TimeSpan.FromHours(5.5);
            ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.TimeSpanValue), inputValue);

            ProcessCycle();

            var outputValue = GetOutputValue<TimeSpan>(nameof(ValueRepeater.ProcessDataOutputs.TimeSpanValue));
            outputValue.Should().Be(inputValue);
        }

        [Fact]
        public void RepeatsChangedDateTimeValue()
        {
            var inputValue1 = new DateTime(2025, 1, 1, 0, 0, 0);
            var inputValue2 = new DateTime(2025, 12, 31, 23, 59, 59);

            ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.DateTimeValue), inputValue1);
            ProcessCycle();
            var outputValue1 = GetOutputValue<DateTime>(nameof(ValueRepeater.ProcessDataOutputs.DateTimeValue));
            ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.DateTimeValue), inputValue2);
            ProcessCycle();
            var outputValue2 = GetOutputValue<DateTime>(nameof(ValueRepeater.ProcessDataOutputs.DateTimeValue));

            outputValue1.Should().Be(inputValue1);
            outputValue2.Should().Be(inputValue2);
        }

        [Fact]
        public void RepeatsChangedTimeSpanValue()
        {
            var inputValue1 = TimeSpan.FromMinutes(30);
            var inputValue2 = TimeSpan.FromHours(2);

            ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.TimeSpanValue), inputValue1);
            ProcessCycle();
            var outputValue1 = GetOutputValue<TimeSpan>(nameof(ValueRepeater.ProcessDataOutputs.TimeSpanValue));
            ChangeInputValue(nameof(ValueRepeater.ProcessDataInputs.TimeSpanValue), inputValue2);
            ProcessCycle();
            var outputValue2 = GetOutputValue<TimeSpan>(nameof(ValueRepeater.ProcessDataOutputs.TimeSpanValue));

            outputValue1.Should().Be(inputValue1);
            outputValue2.Should().Be(inputValue2);
        }
    }
}

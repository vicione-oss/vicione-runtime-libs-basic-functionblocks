using AwesomeAssertions;
using ViciOne.Math;
using Xunit;

public class SinTests
{
    public class OnSettingChanged : RuntimeFunctionBlockTests<Sin>
    {
        [Fact]
        public void CalculatesCorrectResultIfAngleUnitChanged()
        {
            double inputValue = 10;
            ChangeInputValue(nameof(Sin.ProcessDataInputs.Value), inputValue);
            ProcessCycle();
            ChangeSettingValue(nameof(Sin.Settings.InputUnit), AngleUnit.Degrees);
            var expectedOutputValue = System.Math.Sin(inputValue * System.Math.PI / 180.0);

            ProcessCycle();

            var outputValue = GetOutputValue<double>(nameof(Sin.ProcessDataOutputs.Result));
            outputValue.Should().Be(expectedOutputValue);
        }

        [Fact]
        public void DoesNotProcessAngleUnitChangeIfBlockIsNotEnabled()
        {
            ChangeEnabled(false);
            ProcessCycle();
            var outputValueBefore = GetOutputValue<double>(nameof(Sin.ProcessDataOutputs.Result));
            ChangeSettingValue(nameof(Sin.Settings.InputUnit), AngleUnit.Degrees);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<double>(nameof(Sin.ProcessDataOutputs.Result));
            outputValueAfter.Should().Be(outputValueBefore);
        }
    }

    public class ProcessCycle : RuntimeFunctionBlockTests<Sin>
    {
        [Fact]
        public void CalculatesCorrectResultIfInputChanged()
        {
            ChangeSettingValue(nameof(Sin.Settings.InputUnit), AngleUnit.Radians);
            ProcessCycle();
            double inputValue = 10;
            ChangeInputValue(nameof(Sin.ProcessDataInputs.Value), inputValue);
            var expectedOutputValue = System.Math.Sin(inputValue);

            ProcessCycle();

            var outputValue = GetOutputValue<double>(nameof(Sin.ProcessDataOutputs.Result));
            outputValue.Should().Be(expectedOutputValue);
        }

        [Fact]
        public void DoesNotProcessInputIfBlockIsNotEnabled()
        {
            var outputValueBefore = GetOutputValue<double>(nameof(Sin.ProcessDataOutputs.Result));
            ChangeInputValue(nameof(Sin.ProcessDataInputs.Value), 20.0);
            ChangeEnabled(false);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<double>(nameof(Sin.ProcessDataOutputs.Result));
            outputValueAfter.Should().Be(outputValueBefore);
        }
    }

    public class Calculate
    {
        public static TheoryData<double, AngleUnit, double> ArgumentsFor_CalculatesCorrectResult() =>
            new()
            {
            { 5, AngleUnit.Degrees, System.Math.Sin(DegToRad(5)) },
            { 5, AngleUnit.Radians, System.Math.Sin(5) },
            { -5, AngleUnit.Degrees, System.Math.Sin(DegToRad(-5)) },
            { -5, AngleUnit.Radians, System.Math.Sin(-5) },
            { double.MaxValue, AngleUnit.Degrees, System.Math.Sin(DegToRad(double.MaxValue)) },
            { double.MaxValue, AngleUnit.Radians, System.Math.Sin(double.MaxValue) },
            { double.MinValue, AngleUnit.Degrees, System.Math.Sin(DegToRad(double.MinValue)) },
            { double.MinValue, AngleUnit.Radians, System.Math.Sin(double.MinValue) },
            { double.NaN, AngleUnit.Degrees, System.Math.Sin(DegToRad(double.NaN)) },
            { double.NaN, AngleUnit.Radians, System.Math.Sin(double.NaN) },
            { double.NegativeInfinity, AngleUnit.Degrees, System.Math.Sin(DegToRad(double.NegativeInfinity)) },
            { double.NegativeInfinity, AngleUnit.Radians, System.Math.Sin(double.NegativeInfinity) },
            { double.PositiveInfinity, AngleUnit.Degrees, System.Math.Sin(DegToRad(double.PositiveInfinity)) },
            { double.PositiveInfinity, AngleUnit.Radians, System.Math.Sin(double.PositiveInfinity) },
            { double.Epsilon, AngleUnit.Degrees, System.Math.Sin(DegToRad(double.Epsilon)) },
            { double.Epsilon, AngleUnit.Radians, System.Math.Sin(double.Epsilon) },
            };

        [Theory]
        [MemberData(nameof(ArgumentsFor_CalculatesCorrectResult))]
        public void CalculatesCorrectResult(double inputValue, AngleUnit unit, double expectedResult)
        {
            var calculatedResult = Sin.Calculate(inputValue, unit);

            calculatedResult.Should().Be(expectedResult);
        }

        private static double DegToRad(double degrees)
            => degrees * System.Math.PI / 180.0;
    }
}

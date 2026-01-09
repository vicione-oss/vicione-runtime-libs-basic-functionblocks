using AwesomeAssertions;
using ViciOne.Math;
using Xunit;

public class CosTests
{
    public class OnSettingChanged : RuntimeFunctionBlockTests<Cos>
    {
        [Fact]
        public void CalculatesCorrectResultIfAngleUnitChanged()
        {
            double inputValue = 10;
            ChangeInputValue(nameof(Cos.ProcessDataInputs.Value), inputValue);
            ProcessCycle();
            ChangeSettingValue(nameof(Cos.Settings.InputUnit), AngleUnit.Degrees);
            var expectedOutputValue = System.Math.Cos(inputValue * System.Math.PI / 180.0);

            ProcessCycle();

            var outputValue = GetOutputValue<double>(nameof(Cos.ProcessDataOutputs.Result));
            outputValue.Should().Be(expectedOutputValue);
        }

        [Fact]
        public void DoesNotProcessAngleUnitChangeIfBlockIsNotEnabled()
        {
            ChangeEnabled(false);
            ProcessCycle();
            var outputValueBefore = GetOutputValue<double>(nameof(Cos.ProcessDataOutputs.Result));
            ChangeSettingValue(nameof(Cos.Settings.InputUnit), AngleUnit.Degrees);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<double>(nameof(Cos.ProcessDataOutputs.Result));
            outputValueAfter.Should().Be(outputValueBefore);
        }
    }

    public class ProcessCycle : RuntimeFunctionBlockTests<Cos>
    {
        [Fact]
        public void CalculatesCorrectResultIfInputChanged()
        {
            ChangeSettingValue(nameof(Cos.Settings.InputUnit), AngleUnit.Radians);
            ProcessCycle();
            double inputValue = 10;
            ChangeInputValue(nameof(Cos.ProcessDataInputs.Value), inputValue);
            var expectedOutputValue = System.Math.Cos(inputValue);

            ProcessCycle();

            var outputValue = GetOutputValue<double>(nameof(Cos.ProcessDataOutputs.Result));
            outputValue.Should().Be(expectedOutputValue);
        }

        [Fact]
        public void DoesNotProcessInputIfBlockIsNotEnabled()
        {
            var outputValueBefore = GetOutputValue<double>(nameof(Cos.ProcessDataOutputs.Result));
            ChangeInputValue(nameof(Cos.ProcessDataInputs.Value), 20.0);
            ChangeEnabled(false);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<double>(nameof(Cos.ProcessDataOutputs.Result));
            outputValueAfter.Should().Be(outputValueBefore);
        }
    }

    public class Calculate
    {
        public static TheoryData<double, AngleUnit, double> ArgumentsFor_CalculatesCorrectResult() =>
            new()
            {
            { 5, AngleUnit.Degrees, System.Math.Cos(DegToRad(5)) },
            { 5, AngleUnit.Radians, System.Math.Cos(5) },
            { -5, AngleUnit.Degrees, System.Math.Cos(DegToRad(-5)) },
            { -5, AngleUnit.Radians, System.Math.Cos(-5) },
            { double.MaxValue, AngleUnit.Degrees, System.Math.Cos(DegToRad(double.MaxValue)) },
            { double.MaxValue, AngleUnit.Radians, System.Math.Cos(double.MaxValue) },
            { double.MinValue, AngleUnit.Degrees, System.Math.Cos(DegToRad(double.MinValue)) },
            { double.MinValue, AngleUnit.Radians, System.Math.Cos(double.MinValue) },
            { double.NaN, AngleUnit.Degrees, System.Math.Cos(DegToRad(double.NaN)) },
            { double.NaN, AngleUnit.Radians, System.Math.Cos(double.NaN) },
            { double.NegativeInfinity, AngleUnit.Degrees, System.Math.Cos(DegToRad(double.NegativeInfinity)) },
            { double.NegativeInfinity, AngleUnit.Radians, System.Math.Cos(double.NegativeInfinity) },
            { double.PositiveInfinity, AngleUnit.Degrees, System.Math.Cos(DegToRad(double.PositiveInfinity)) },
            { double.PositiveInfinity, AngleUnit.Radians, System.Math.Cos(double.PositiveInfinity) },
            { double.Epsilon, AngleUnit.Degrees, System.Math.Cos(DegToRad(double.Epsilon)) },
            { double.Epsilon, AngleUnit.Radians, System.Math.Cos(double.Epsilon) },
            };

        [Theory]
        [MemberData(nameof(ArgumentsFor_CalculatesCorrectResult))]
        public void CalculatesCorrectResult(double inputValue, AngleUnit unit, double expectedResult)
        {
            var calculatedResult = Cos.Calculate(inputValue, unit);

            calculatedResult.Should().Be(expectedResult);
        }

        private static double DegToRad(double degrees)
            => degrees * System.Math.PI / 180.0;
    }
}

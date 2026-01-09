using AwesomeAssertions;
using ViciOne.Math;
using Xunit;

public class TanTests
{
    public class OnSettingChanged : RuntimeFunctionBlockTests<Tan>
    {
        [Fact]
        public void CalculatesCorrectResultIfAngleUnitChanged()
        {
            double inputValue = 10;
            ChangeInputValue(nameof(Tan.ProcessDataInputs.Value), inputValue);
            ProcessCycle();
            ChangeSettingValue(nameof(Tan.Settings.InputUnit), AngleUnit.Degrees);
            var expectedOutputValue = System.Math.Tan(inputValue * System.Math.PI / 180.0);

            ProcessCycle();

            var outputValue = GetOutputValue<double>(nameof(Tan.ProcessDataOutputs.Result));
            outputValue.Should().Be(expectedOutputValue);
        }

        [Fact]
        public void DoesNotProcessAngleUnitChangeIfBlockIsNotEnabled()
        {
            ChangeEnabled(false);
            ProcessCycle();
            var outputValueBefore = GetOutputValue<double>(nameof(Tan.ProcessDataOutputs.Result));
            ChangeSettingValue(nameof(Tan.Settings.InputUnit), AngleUnit.Degrees);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<double>(nameof(Tan.ProcessDataOutputs.Result));
            outputValueAfter.Should().Be(outputValueBefore);
        }
    }

    public class ProcessCycle : RuntimeFunctionBlockTests<Tan>
    {
        [Fact]
        public void CalculatesCorrectResultIfInputChanged()
        {
            ChangeSettingValue(nameof(Tan.Settings.InputUnit), AngleUnit.Radians);
            ProcessCycle();
            double inputValue = 10;
            ChangeInputValue(nameof(Tan.ProcessDataInputs.Value), inputValue);
            var expectedOutputValue = System.Math.Tan(inputValue);

            ProcessCycle();

            var outputValue = GetOutputValue<double>(nameof(Tan.ProcessDataOutputs.Result));
            outputValue.Should().Be(expectedOutputValue);
        }

        [Theory]
        [InlineData(5, AngleUnit.Degrees, false)]
        [InlineData(-5, AngleUnit.Degrees, false)]
        [InlineData(double.MaxValue, AngleUnit.Degrees, true)]
        [InlineData(double.MinValue, AngleUnit.Degrees, true)]
        [InlineData(double.NaN, AngleUnit.Degrees, true)]
        [InlineData(double.NegativeInfinity, AngleUnit.Degrees, true)]
        [InlineData(double.PositiveInfinity, AngleUnit.Degrees, true)]
        [InlineData(0, AngleUnit.Degrees, false)]
        [InlineData(90, AngleUnit.Degrees, true)]
        [InlineData(270, AngleUnit.Degrees, true)]
        [InlineData(-90, AngleUnit.Degrees, true)]
        [InlineData(-270, AngleUnit.Degrees, true)]
        [InlineData(0.1, AngleUnit.Degrees, false)]
        [InlineData(89.9, AngleUnit.Degrees, false)]
        [InlineData(269.9, AngleUnit.Degrees, false)]
        [InlineData(-89.9, AngleUnit.Degrees, false)]
        [InlineData(-260.0, AngleUnit.Degrees, false)]
        [InlineData(5, AngleUnit.Radians, false)]
        [InlineData(-5, AngleUnit.Radians, false)]
        [InlineData(double.MaxValue, AngleUnit.Radians, false)]
        [InlineData(double.MinValue, AngleUnit.Radians, false)]
        [InlineData(double.NaN, AngleUnit.Radians, true)]
        [InlineData(double.NegativeInfinity, AngleUnit.Radians, true)]
        [InlineData(double.PositiveInfinity, AngleUnit.Radians, true)]
        [InlineData(0, AngleUnit.Radians, false)]
        [InlineData(System.Math.PI, AngleUnit.Radians, false)]
        [InlineData(2 * System.Math.PI, AngleUnit.Radians, false)]
        [InlineData(0.5 * System.Math.PI, AngleUnit.Radians, true)]
        [InlineData(1.5 * System.Math.PI, AngleUnit.Radians, true)]
        [InlineData(-System.Math.PI, AngleUnit.Radians, false)]
        [InlineData(2 * -System.Math.PI, AngleUnit.Radians, false)]
        [InlineData(0.5 * -System.Math.PI, AngleUnit.Radians, true)]
        [InlineData(1.5 * -System.Math.PI, AngleUnit.Radians, true)]
        public void CalculatesCorrectNaNIfInputChanged(double inputValue, AngleUnit unit, bool expectedResult)
        {
            ChangeSettingValue(nameof(Tan.Settings.InputUnit), unit);
            ProcessCycle();
            ChangeInputValue(nameof(Tan.ProcessDataInputs.Value), inputValue);

            ProcessCycle();

            var outputValue = GetOutputValue<bool>(nameof(Tan.ProcessDataOutputs.NaN));
            outputValue.Should().Be(expectedResult);
        }

        [Fact]
        public void DoesNotProcessInputIfBlockIsNotEnabled()
        {
            var outputValueBefore = GetOutputValue<double>(nameof(Tan.ProcessDataOutputs.Result));
            ChangeInputValue(nameof(Tan.ProcessDataInputs.Value), 20.0);
            ChangeEnabled(false);

            ProcessCycle();

            var outputValueAfter = GetOutputValue<double>(nameof(Tan.ProcessDataOutputs.Result));
            outputValueAfter.Should().Be(outputValueBefore);
        }
    }

    public class Calculate
    {
        public static TheoryData<double, AngleUnit, double> ArgumentsFor_CalculatesCorrectResult() =>
            new()
            {
            { 5, AngleUnit.Degrees, System.Math.Tan(DegToRad(5)) },
            { -5, AngleUnit.Degrees, System.Math.Tan(DegToRad(-5)) },
            { double.MaxValue, AngleUnit.Degrees, System.Math.Tan(DegToRad(double.MaxValue)) },
            { double.MinValue, AngleUnit.Degrees, System.Math.Tan(DegToRad(double.MaxValue)) },
            { double.NaN, AngleUnit.Degrees, double.NaN },
            { double.NegativeInfinity, AngleUnit.Degrees, double.NaN },
            { double.PositiveInfinity, AngleUnit.Degrees, double.NaN },
            { 0, AngleUnit.Degrees, System.Math.Tan(DegToRad(0)) },
            { 90, AngleUnit.Degrees, double.NaN },
            { 180, AngleUnit.Degrees, System.Math.Tan(DegToRad(180)) },
            { 270, AngleUnit.Degrees, double.NaN },
            { 360, AngleUnit.Degrees, System.Math.Tan(DegToRad(360)) },
            { -90, AngleUnit.Degrees, double.NaN },
            { -180, AngleUnit.Degrees, System.Math.Tan(DegToRad(-180)) },
            { -270, AngleUnit.Degrees, double.NaN },
            { -360, AngleUnit.Degrees, System.Math.Tan(DegToRad(-360)) },
            { 5, AngleUnit.Radians, System.Math.Tan(5) },
            { -5, AngleUnit.Radians, System.Math.Tan(-5) },
            { double.MaxValue, AngleUnit.Radians, System.Math.Tan(double.MaxValue) },
            { double.MinValue, AngleUnit.Radians, System.Math.Tan(double.MinValue) },
            { double.NaN, AngleUnit.Radians, double.NaN },
            { double.NegativeInfinity, AngleUnit.Radians, double.NaN },
            { double.PositiveInfinity, AngleUnit.Radians, double.NaN },
            { 0, AngleUnit.Radians, System.Math.Tan(0) },
            { System.Math.PI, AngleUnit.Radians, System.Math.Tan(System.Math.PI) },
            { 2 * System.Math.PI, AngleUnit.Radians, System.Math.Tan(2 * System.Math.PI) },
            { 0.5 * System.Math.PI, AngleUnit.Radians, double.NaN },
            { 1.5 * System.Math.PI, AngleUnit.Radians, double.NaN },
            { -System.Math.PI, AngleUnit.Radians, System.Math.Tan(-System.Math.PI) },
            { 2 * -System.Math.PI, AngleUnit.Radians, System.Math.Tan(2 * -System.Math.PI) },
            { 0.5 * -System.Math.PI, AngleUnit.Radians, double.NaN },
            { 1.5 * -System.Math.PI, AngleUnit.Radians, double.NaN },
            };

        [Theory]
        [MemberData(nameof(ArgumentsFor_CalculatesCorrectResult))]
        public void CalculatesCorrectResult(double inputValue, AngleUnit unit, double expectedResult)
        {
            var calculatedResult = Tan.Calculate(inputValue, unit);

            calculatedResult.Should().Be(expectedResult);
        }

        private static double DegToRad(double degrees)
            => degrees * System.Math.PI / 180.0;
    }
}

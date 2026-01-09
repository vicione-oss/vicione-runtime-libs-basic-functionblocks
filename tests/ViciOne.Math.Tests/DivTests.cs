using AwesomeAssertions;
using ViciOne.Math;
using Xunit;

public class DivTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<Div>
    {
        [Fact]
        public void CalculatesCorrectResult()
        {
            ChangeInputValue(nameof(Div.ProcessDataInputs.Dividend), 15.0);
            ChangeInputValue(nameof(Div.ProcessDataInputs.Divisor), 5.0);

            ProcessCycle();

            var quotient = GetOutputValue<double>(nameof(Div.ProcessDataOutputs.Quotient));
            quotient.Should().Be(3.0);
        }

        [Fact]
        public void DoesNothingIfBlockIsNotEnabled()
        {
            var quotientBefore = GetOutputValue<double>(nameof(Div.ProcessDataOutputs.Quotient));
            ChangeInputValue(nameof(Div.ProcessDataInputs.Dividend), 15.0);
            ChangeInputValue(nameof(Div.ProcessDataInputs.Divisor), 5.0);
            ChangeEnabled(false);

            ProcessCycle();

            var quotient = GetOutputValue<double>(nameof(Div.ProcessDataOutputs.Quotient));
            quotient.Should().Be(quotientBefore);
        }

        [Fact]
        public void SetsQuotientToZeroIfDivisorIsZero()
        {
            ChangeInputValue(nameof(Div.ProcessDataInputs.Dividend), 15.0);
            ChangeInputValue(nameof(Div.ProcessDataInputs.Divisor), 0.0);

            ProcessCycle();

            var quotient = GetOutputValue<double>(nameof(Div.ProcessDataOutputs.Quotient));
            quotient.Should().Be(0.0);
        }
    }
}

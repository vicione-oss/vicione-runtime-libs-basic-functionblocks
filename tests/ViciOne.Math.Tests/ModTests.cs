using AwesomeAssertions;
using ViciOne.Math;
using Xunit;

public class ModTests
{
    public class ProcessCycle : RuntimeFunctionBlockTests<Mod>
    {
        [Fact]
        public void CalculatesCorrectResult()
        {
            ChangeInputValue(nameof(Mod.ProcessDataInputs.Dividend), 13.0);
            ChangeInputValue(nameof(Mod.ProcessDataInputs.Divisor), 5.0);

            ProcessCycle();

            var quotient = GetOutputValue<double>(nameof(Mod.ProcessDataOutputs.Remainder));
            quotient.Should().Be(3.0);
        }

        [Fact]
        public void DoesNothingIfBlockIsNotEnabled()
        {
            var quotientBefore = GetOutputValue<double>(nameof(Mod.ProcessDataOutputs.Remainder));
            ChangeInputValue(nameof(Mod.ProcessDataInputs.Dividend), 13.0);
            ChangeInputValue(nameof(Mod.ProcessDataInputs.Divisor), 5.0);
            ChangeEnabled(false);

            ProcessCycle();

            var quotient = GetOutputValue<double>(nameof(Mod.ProcessDataOutputs.Remainder));
            quotient.Should().Be(quotientBefore);
        }

        [Fact]
        public void SetsQuotientToZeroIfDivdisorIsZero()
        {
            ChangeInputValue(nameof(Mod.ProcessDataInputs.Dividend), 13.0);
            ChangeInputValue(nameof(Mod.ProcessDataInputs.Divisor), 0.0);

            ProcessCycle();

            var quotient = GetOutputValue<double>(nameof(Mod.ProcessDataOutputs.Remainder));
            quotient.Should().Be(0.0);
        }
    }
}

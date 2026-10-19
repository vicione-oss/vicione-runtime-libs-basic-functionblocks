using AwesomeAssertions;
using ViciOne.Analysis;
using Xunit;

public class IntervalAccumulatorTests
{
    public class Complete
    {
        [Fact]
        public void ReturnsInitializationValueIfNoValueWasAdded()
        {
            IntervalAccumulator accumulator = new(3.0, 0);

            IntervalResult result = accumulator.Complete(100);

            result.Should().Be(new IntervalResult(3.0, 3.0, 3.0, 0, 0));
        }

        [Fact]
        public void WeightsAverageByDuration()
        {
            IntervalAccumulator accumulator = new(0, 0);
            accumulator.Add(10, 25);
            accumulator.Add(20, 75);

            IntervalResult result = accumulator.Complete(100);

            // 0 * 25 + 10 * 50 + 20 * 25 = 1000 → 1000 / 100
            result.Average.Should().BeApproximately(10.0, 1e-9);
            result.Minimum.Should().Be(0);
            result.Maximum.Should().Be(20);
            result.Sum.Should().Be(30);
        }

        [Fact]
        public void KeepsAverageOfConstantInputExact()
        {
            IntervalAccumulator accumulator = new(0.1, 0);
            for (int t = 3; t < 1000; t += 7)
                accumulator.Add(0.1, t);

            accumulator.Complete(1000).Average.Should().Be(0.1);
        }

        [Fact]
        public void CarriesLastValueIntoNextInterval()
        {
            IntervalAccumulator accumulator = new(1, 0);
            accumulator.Add(5, 50);
            accumulator.Complete(100);

            IntervalResult result = accumulator.Complete(200);

            result.Should().Be(new IntervalResult(5, 5, 5, 0, 0));
        }

        [Fact]
        public void UsesSecondToLastValueForFirstDifferenceAndLastValueAfterwards()
        {
            IntervalAccumulator accumulator = new(1, 0);
            accumulator.Complete(100).Difference.Should().Be(0);

            accumulator.Add(2, 110);
            accumulator.Add(4, 120);
            accumulator.Add(7, 130);
            // First real difference: first value minus second-to-last value
            accumulator.Complete(200).Difference.Should().Be(1 - 4);

            accumulator.Add(10, 210);
            // Afterwards: first value (carried over) minus last value
            accumulator.Complete(300).Difference.Should().Be(7 - 10);
        }

        [Fact]
        public void CompletesAtLastValueIfValueWasAddedAfterTick()
        {
            IntervalAccumulator accumulator = new(2, 0);
            accumulator.Add(4, 150);

            IntervalResult result = accumulator.Complete(100);

            // 2 * 150 + 4 * 0 → 300 / 150; the next interval starts at 150
            result.Average.Should().BeApproximately(2.0, 1e-9);
            accumulator.Complete(250).Average.Should().Be(4);
        }
    }

    public class NaNInitializationValue
    {
        [Fact]
        public void ReturnsZeroIfNoValueWasAdded()
        {
            IntervalAccumulator accumulator = new(double.NaN, 0);

            accumulator.Complete(100).Should().Be(new IntervalResult(0, 0, 0, 0, 0));
        }

        [Fact]
        public void StartsIntervalWithFirstValue()
        {
            IntervalAccumulator accumulator = new(double.NaN, 0);
            accumulator.Add(5, 50);

            accumulator.Complete(100).Should().Be(new IntervalResult(5, 5, 5, 0, 0));
        }

        [Fact]
        public void WeightsAverageFromFirstValue()
        {
            IntervalAccumulator accumulator = new(double.NaN, 0);
            accumulator.Add(4, 50);
            accumulator.Add(8, 75);

            IntervalResult result = accumulator.Complete(100);

            // 4 * 25 + 8 * 25 → 300 / 50
            result.Average.Should().BeApproximately(6, 1e-9);
            result.Minimum.Should().Be(4);
            result.Sum.Should().Be(8);
        }

        [Fact]
        public void KeepsFirstDifferenceRuleForNextInterval()
        {
            IntervalAccumulator accumulator = new(double.NaN, 0);
            accumulator.Add(5, 50);
            accumulator.Complete(100);

            accumulator.Add(2, 110);
            accumulator.Add(4, 120);
            accumulator.Add(7, 130);

            accumulator.Complete(200).Difference.Should().Be(5 - 4);
        }
    }

    public class Add
    {
        [Fact]
        public void IgnoresNaN()
        {
            IntervalAccumulator accumulator = new(1, 0);
            accumulator.Add(double.NaN, 50);

            IntervalResult result = accumulator.Complete(100);

            result.Should().Be(new IntervalResult(1, 1, 1, 0, 0));
        }
    }
}

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
            var accumulator = new IntervalAccumulator(3.0, 0);

            var result = accumulator.Complete(100);

            result.Should().Be(new IntervalResult(3.0, 3.0, 3.0, 0, 0, 3.0));
        }

        [Fact]
        public void WeightsAverageByDuration()
        {
            var accumulator = new IntervalAccumulator(0, 0);
            accumulator.Add(10, 25);
            accumulator.Add(20, 75);

            var result = accumulator.Complete(100);

            // 0 * 25 + 10 * 50 + 20 * 25 = 1000 → 1000 / 100
            result.Average.Should().BeApproximately(10.0, 1e-9);
            result.Minimum.Should().Be(0);
            result.Maximum.Should().Be(20);
            result.Sum.Should().Be(30);
            result.Last.Should().Be(20);
        }

        [Fact]
        public void CarriesLastValueIntoNextInterval()
        {
            var accumulator = new IntervalAccumulator(1, 0);
            accumulator.Add(5, 50);
            accumulator.Complete(100);

            var result = accumulator.Complete(200);

            result.Should().Be(new IntervalResult(5, 5, 5, 0, 0, 5));
        }

        [Fact]
        public void CalculatesDifferenceLikePreviousImplementation()
        {
            var accumulator = new IntervalAccumulator(1, 0);
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
            var accumulator = new IntervalAccumulator(2, 0);
            accumulator.Add(4, 150);

            var result = accumulator.Complete(100);

            // 2 * 150 + 4 * 0 → 300 / 150; the next interval starts at 150
            result.Average.Should().BeApproximately(2.0, 1e-9);
            accumulator.Complete(250).Average.Should().Be(4);
        }
    }

    public class Add
    {
        [Fact]
        public void IgnoresNaN()
        {
            var accumulator = new IntervalAccumulator(1, 0);
            accumulator.Add(double.NaN, 50);

            var result = accumulator.Complete(100);

            result.Should().Be(new IntervalResult(1, 1, 1, 0, 0, 1));
        }
    }
}

using AwesomeAssertions;
using ViciOne.Analysis;
using Xunit;

public class IntervalStatisticTests
{
    private const int CompressionTime = 1000;

    public class ProcessCycle : RuntimeFunctionBlockTests<IntervalStatistic>
    {
        private readonly ManualTimeProvider _time = new();
        private readonly IntervalScheduler _scheduler;

        public ProcessCycle()
            => _scheduler = new IntervalScheduler(_time, false);

        private void Start(double initialValue)
        {
            ChangeSettingValue(nameof(IntervalStatistic.Settings.CompressionTime), CompressionTime);
            ChangeInputValue(nameof(IntervalStatistic.ProcessDataInputs.Value), initialValue);
            StartEngine();
            ProcessCycle();
        }

        private void CompleteInterval(int milliseconds = CompressionTime)
        {
            _time.Advance(milliseconds);
            _scheduler.RunDueGroups();
            ProcessCycle();
        }

        private double Output(string name)
            => GetOutputValue<double>(name);

        protected override void StartEngine()
        {
            IntervalStatistic.SchedulerOverride = _scheduler;
            try
            {
                base.StartEngine();
            }
            finally
            {
                IntervalStatistic.SchedulerOverride = null;
            }

            GetRuntimeFunctionBlock().Scheduler.Should().BeSameAs(_scheduler);
        }

        [Fact]
        public void DoesNotRegisterIfInputWasNotChanged()
        {
            ChangeSettingValue(nameof(IntervalStatistic.Settings.CompressionTime), CompressionTime);
            StartEngine();
            ProcessCycle();

            CompleteInterval();

            Output(nameof(IntervalStatistic.ProcessDataOutputs.Average)).Should().Be(0);
            Output(nameof(IntervalStatistic.ProcessDataOutputs.Last)).Should().Be(0);
        }

        [Fact]
        public void PassesInputValueToAverageOutputOnFirstInterval()
        {
            Start(2.0);
            _time.Advance(500);
            ChangeInputValue(nameof(IntervalStatistic.ProcessDataInputs.Value), 4.0);
            ProcessCycle();

            CompleteInterval(500);

            Output(nameof(IntervalStatistic.ProcessDataOutputs.Average)).Should().Be(4.0);
            Output(nameof(IntervalStatistic.ProcessDataOutputs.Last)).Should().Be(4.0);
            Output(nameof(IntervalStatistic.ProcessDataOutputs.Minimum)).Should().Be(2.0);
            Output(nameof(IntervalStatistic.ProcessDataOutputs.Maximum)).Should().Be(4.0);
            Output(nameof(IntervalStatistic.ProcessDataOutputs.Sum)).Should().Be(4.0);
        }

        [Fact]
        public void UpdatesOutputsWithTimeWeightedStatisticsOnSecondInterval()
        {
            Start(2.0);
            CompleteInterval();
            _time.Advance(250);
            ChangeInputValue(nameof(IntervalStatistic.ProcessDataInputs.Value), 6.0);
            ProcessCycle();

            CompleteInterval(750);

            Output(nameof(IntervalStatistic.ProcessDataOutputs.Average)).Should().BeApproximately(5.0, 1e-9);
            Output(nameof(IntervalStatistic.ProcessDataOutputs.Minimum)).Should().Be(2.0);
            Output(nameof(IntervalStatistic.ProcessDataOutputs.Maximum)).Should().Be(6.0);
            Output(nameof(IntervalStatistic.ProcessDataOutputs.Sum)).Should().Be(6.0);
            Output(nameof(IntervalStatistic.ProcessDataOutputs.Last)).Should().Be(6.0);
        }

        [Fact]
        public void UpdatesOutputsWithNegativeValue()
        {
            Start(-1.0);
            CompleteInterval();
            CompleteInterval();

            Output(nameof(IntervalStatistic.ProcessDataOutputs.Average)).Should().Be(-1.0);
            Output(nameof(IntervalStatistic.ProcessDataOutputs.Minimum)).Should().Be(-1.0);
            Output(nameof(IntervalStatistic.ProcessDataOutputs.Maximum)).Should().Be(-1.0);
            Output(nameof(IntervalStatistic.ProcessDataOutputs.Last)).Should().Be(-1.0);
        }

        [Fact]
        public void UpdatesOutputsIfInitialValueDiffersFromDesignDefault()
        {
            Start(double.MaxValue);

            CompleteInterval();

            Output(nameof(IntervalStatistic.ProcessDataOutputs.Average)).Should().Be(double.MaxValue);
            Output(nameof(IntervalStatistic.ProcessDataOutputs.Last)).Should().Be(double.MaxValue);
        }

        [Fact]
        public void DiscardsResultIfBlockIsNotEnabled()
        {
            Start(2.0);
            ChangeEnabled(false);
            ProcessCycle();

            CompleteInterval();
            ChangeEnabled(true);
            ProcessCycle();

            Output(nameof(IntervalStatistic.ProcessDataOutputs.Average)).Should().Be(0);
            Output(nameof(IntervalStatistic.ProcessDataOutputs.Last)).Should().Be(0);
        }

        [Fact]
        public void SetsCurrentInputValueAsLastIfValueChangedWhileDisabled()
        {
            Start(2.0);
            CompleteInterval();
            ChangeEnabled(false);
            ProcessCycle();
            ChangeInputValue(nameof(IntervalStatistic.ProcessDataInputs.Value), 5.0);
            ProcessCycle();
            ChangeEnabled(true);
            ProcessCycle();

            CompleteInterval();

            Output(nameof(IntervalStatistic.ProcessDataOutputs.Last)).Should().Be(5.0);
            // Changes while the block is disabled are not aggregated
            Output(nameof(IntervalStatistic.ProcessDataOutputs.Maximum)).Should().Be(2.0);
            Output(nameof(IntervalStatistic.ProcessDataOutputs.Average)).Should().Be(2.0);
        }

        [Fact]
        public void SetsNaNAsLastButIgnoresItInStatistics()
        {
            Start(2.0);
            CompleteInterval();
            ChangeInputValue(nameof(IntervalStatistic.ProcessDataInputs.Value), double.NaN);
            ProcessCycle();

            CompleteInterval();

            Output(nameof(IntervalStatistic.ProcessDataOutputs.Last)).Should().Be(double.NaN);
            Output(nameof(IntervalStatistic.ProcessDataOutputs.Average)).Should().Be(2.0);
            Output(nameof(IntervalStatistic.ProcessDataOutputs.Maximum)).Should().Be(2.0);
        }

        [Fact]
        public void UsesNewCompressionTimeAfterSettingChanged()
        {
            Start(2.0);
            ChangeSettingValue(nameof(IntervalStatistic.Settings.CompressionTime), 5000);
            ProcessCycle();

            CompleteInterval();
            Output(nameof(IntervalStatistic.ProcessDataOutputs.Last)).Should().Be(0);

            CompleteInterval(4000);
            Output(nameof(IntervalStatistic.ProcessDataOutputs.Last)).Should().Be(2.0);
        }

        [Fact]
        public void CountsTickOverwrittenWithoutEngineCycle()
        {
            Start(2.0);

            _time.Advance(CompressionTime);
            _scheduler.RunDueGroups();
            _time.Advance(CompressionTime);
            _scheduler.RunDueGroups();

            GetRuntimeFunctionBlock().MissedIntervals.Should().Be(1);
        }

        [Fact]
        public void StopsReceivingIntervalsAfterDispose()
        {
            Start(2.0);
            IntervalStatistic block = GetRuntimeFunctionBlock();

            block.Dispose();

            _time.Advance(CompressionTime);
            _scheduler.RunDueGroups().Should().Be(long.MaxValue);
        }
    }
}

using System;
using System.Timers;
using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Time;

public partial class Scheduler
{
    private const int Interval = 15 * 1000;

    private SchedulerCore? _scheduler;
    private bool _setTriggerOutputToTrue;
    private Timer? _timer;

    internal void CheckScheduler(DateTime dateTime)
    {
        if (_scheduler is not null && _scheduler.CheckForMatchingSchedule(dateTime))
        {
            _setTriggerOutputToTrue = true;
            MarkAsChanged();
        }
    }

    internal void DeactivateTimer() => _timer?.Stop();

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (_timer is not null)
        {
            _timer.Elapsed -= Timer_Elapsed;
            _timer.Dispose();
        }
    }

    public override void InitializeII()
    {
        base.InitializeII();

        _scheduler ??= new SchedulerCore();

        if (_timer is null)
        {
            _timer = new Timer(Interval);
            _timer.Elapsed += Timer_Elapsed;
            _timer.Enabled = true;
        }
    }

    protected override void OnSettingChanged(IRuntimeSetting setting)
    {
        if (setting.Setting.Id != Settings.Times.Id)
            return;

        _scheduler?.ParseTimes(Settings.Times.Value, Logger);
    }

    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
    {
        if (FunctionBlockEnabled is false)
            return;

        if (_setTriggerOutputToTrue)
        {
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Trigger, true);
            _setTriggerOutputToTrue = false;
            MarkAsChanged();
        }
        else
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Trigger, false);
    }

    private void Timer_Elapsed(object? sender, ElapsedEventArgs e) => CheckScheduler(e.SignalTime);
}

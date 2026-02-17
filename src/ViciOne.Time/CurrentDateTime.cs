using System;
using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Time;

public partial class CurrentDateTime
{
    private void Publish()
    {
        if (FunctionBlockEnabled)
            SetConnectorValueIfNotEqual(ProcessDataOutputs.Now, GetNow(Settings.UseUtc.Value));
    }

    internal static DateTime GetNow(bool useUtc)
        => useUtc
            ? DateTime.UtcNow
            : DateTime.Now;

    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
        => Publish();
}

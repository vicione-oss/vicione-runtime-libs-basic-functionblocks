using System;
using System.Security.Cryptography;
using ViciOne.Core.Contracts.DataModel;
using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Math;

public partial class Random
{
    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
    {
        if (FunctionBlockEnabled is false)
            return;

        if (ShouldGenerate())
        {
            int randomNumber = RandomNumberGenerator.GetInt32(Settings.Minimum.Value, Settings.Maximum.Value);

            SetConnectorValueIfNotEqual(ProcessDataOutputs.NextLong, randomNumber);
            SetConnectorValueIfNotEqual(ProcessDataOutputs.NextDouble, System.Math.Min(randomNumber + NextDouble(), Settings.Maximum.Value));
        }

        bool ShouldGenerate()
        {
            if (FunctionBlock.RunMode == RunMode.Cyclic)
                return true;
            if (ProcessDataInputs.Trigger.Value is false)
                return false;
            return ProcessDataInputs.Trigger.Changed || cycleInfo.IsFirstCycle;
        }

        static double NextDouble()
        {
            ulong nextULong = BitConverter.ToUInt64(RandomNumberGenerator.GetBytes(sizeof(ulong)));
            return (nextULong >> 11) * (1.0 / (1ul << 53));
        }
    }
}

using System;
using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Math;

public partial class Equality
{
    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
    {
        EqualityResult result = EqualityResult.None;
        if (FunctionBlockEnabled)
        {
            bool useDoubleInputs =
                ProcessDataInputs.Double0.Changed ||
                ProcessDataInputs.Double1.Changed;

            if (useDoubleInputs == false &&
                ProcessDataInputs.Long0.Changed == false &&
                ProcessDataInputs.Long1.Changed == false)
                useDoubleInputs = true;

            result = useDoubleInputs
                ? GetResult(ProcessDataInputs.Double0.Value, ProcessDataInputs.Double1.Value)
                : GetResult(ProcessDataInputs.Long0.Value, ProcessDataInputs.Long1.Value);
        }

        SetConnectorValueIfNotEqual(ProcessDataOutputs.Equal, result == EqualityResult.Equal);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.Unequal, result is EqualityResult.GreaterThan or EqualityResult.LessThan);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.GreaterThan, result == EqualityResult.GreaterThan);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.LessThan, result == EqualityResult.LessThan);
    }

    private static EqualityResult GetResult(IComparable value1, IComparable value2)
    {
        int comparisonResult = value1.CompareTo(value2);
        if (comparisonResult == 0)
            return EqualityResult.Equal;
        else if (comparisonResult > 0)
            return EqualityResult.GreaterThan;
        else
            return EqualityResult.LessThan;
    }
}

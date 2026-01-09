using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.FlowControl;

/// <summary>
/// Funktionsbaustein, der die aktuellen Werte aller Eingangskonnektoren in jedem Zyklus an die entsprechenden Ausgangskonnektoren weitergibt.
/// </summary>
/// <remarks>
/// Für jeden unterstützten primitiven Eingangskonnektor (und <see cref="string"/>) existiert ein passender Ausgangskonnektor mit demselben Datentyp.
/// In jedem Verarbeitungszyklus (wenn aktiviert) kopiert der Baustein den Eingangswert zum Ausgang mittels <c>SetConnectorValueIfNotEqual</c>.
/// Der Baustein führt eine einfache 1:1-Durchleitung durch; es wird keine Transformation oder Validierung angewendet, außer Gleichheitsprüfungen.
/// </remarks>
public partial class ValueRepeater
{
    /// <summary>
    /// Führt einen einzelnen Zyklus aus und gibt den aktuellen Wert jedes Eingangs an den entsprechenden Ausgang weiter, falls sich der Wert geändert hat.
    /// </summary>
    /// <param name="cycleInfo">Von der Laufzeitumgebung bereitgestellte Metadaten für den aktuellen Zyklus (in dieser Implementierung ungenutzt).</param>
    /// <remarks>
    /// Kehrt sofort zurück, wenn der Funktionsbaustein deaktiviert ist (Aktivierungs-Flag der Basisklasse). Für jedes Eingangs-/Ausgangspaar führt die Methode folgende Schritte aus:
    /// 1. Liest den aktuellen <c>Value</c> des Eingangskonnektors.
    /// 2. Ruft <c>SetConnectorValueIfNotEqual</c> auf dem entsprechenden Ausgang auf, um den neuen Wert nur bei Änderung zu veröffentlichen.
    /// </remarks>
    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
    {
        if (!FunctionBlockEnabled)
            return;
        SetConnectorValueIfNotEqual(ProcessDataOutputs.DoubleValue, ProcessDataInputs.DoubleValue.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.BoolValue, ProcessDataInputs.BoolValue.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.ByteValue, ProcessDataInputs.ByteValue.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.SByteValue, ProcessDataInputs.SByteValue.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.CharValue, ProcessDataInputs.CharValue.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.ShortValue, ProcessDataInputs.ShortValue.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.UShortValue, ProcessDataInputs.UShortValue.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.IntValue, ProcessDataInputs.IntValue.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.UIntValue, ProcessDataInputs.UIntValue.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.LongValue, ProcessDataInputs.LongValue.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.ULongValue, ProcessDataInputs.ULongValue.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.FloatValue, ProcessDataInputs.FloatValue.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.DecimalValue, ProcessDataInputs.DecimalValue.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.StringValue, ProcessDataInputs.StringValue.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.DateTimeValue, ProcessDataInputs.DateTimeValue.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.TimeSpanValue, ProcessDataInputs.TimeSpanValue.Value);
    }
}

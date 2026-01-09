using ViciOne.Core.Contracts.Runtime;

namespace ViciOne.Constant;

/// <summary>
///     Funktionsbaustein, der zyklisch zwischen zwei konfigurierbaren Konstantenwerten für alle unterstützten primitiven Datentypen wechselt.
/// </summary>
/// <remarks>
///     Für jeden unterstützten .NET-Primitiven (und <see cref="string" />) existieren zwei Einstellungen: <c>TypeValueOne</c> und
///     <c>TypeValueTwo</c>.
///     In jedem Ausführungszyklus (wenn aktiviert) wechselt der Baustein zwischen den *One und *Two Werten am Ausgang.
///     Der Wechsel wird durch ein internes boolesches Flag gesteuert: Bei true werden die *One-Werte ausgegeben, bei false
///     die *Two-Werte.
///     Jedes Wertepaar ist mit einem Standardwert initialisiert (über <c>[DefaultValue]</c>), soweit vorhanden — diese Standardwerte
///     können zur Entwurfs- und Laufzeit frei konfiguriert werden. (Decimal-Paare haben bewusst keinen Standardwert zugewiesen.)
/// </remarks>
public partial class AlternatingValues
{
    /// <summary>
    ///     Internes Wechsel-Flag, das bestimmt, welcher Wertesatz (ValueOne oder ValueTwo) im aktuellen Zyklus ausgegeben wird.
    ///     true = ValueOne, false = ValueTwo. Wird am Ende jeder erfolgreichen <see cref="ProcessCycle" /> Ausführung umgeschaltet.
    /// </summary>
    public bool UseValueOne { get; set; } = true;

    /// <summary>
    ///     Führt einen Verarbeitungszyklus aus: Wechselt jeden Ausgang zwischen den konfigurierten <c>ValueOne</c> und
    ///     <c>ValueTwo</c> Einstellungen.
    /// </summary>
    /// <param name="cycleInfo">Metadaten über den aktuellen Laufzeitzyklus, bereitgestellt von der Ausführungsumgebung.</param>
    /// <remarks>
    ///     Die Methode kehrt sofort zurück, wenn der Funktionsbaustein deaktiviert ist (siehe Aktivierungs-Flag der Basisklasse).
    ///     Für jeden primitiven Typ + String führt die Methode folgende Schritte aus:
    ///     1. Bestimmt basierend auf dem aktuellen Wert von <see cref="UseValueOne" />, welche Einstellung verwendet wird.
    ///     2. Schreibt den ausgewählten Wert an den Ausgangskonnektor, nur wenn dieser vom aktuellen Konnektorwert abweicht.
    ///     Nach Verarbeitung aller Ausgänge wird das Wechsel-Flag umgeschaltet.
    /// </remarks>
    protected override void ProcessCycle(IFunctionBlockCycleInfo cycleInfo)
    {
        if (!FunctionBlockEnabled)
            return;

        SetConnectorValueIfNotEqual(ProcessDataOutputs.BoolValue,
            UseValueOne ? Settings.BoolValueOne.Value : Settings.BoolValueTwo.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.DoubleValue,
            UseValueOne ? Settings.DoubleValueOne.Value : Settings.DoubleValueTwo.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.ByteValue,
            UseValueOne ? Settings.ByteValueOne.Value : Settings.ByteValueTwo.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.SByteValue,
            UseValueOne ? Settings.SByteValueOne.Value : Settings.SByteValueTwo.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.CharValue,
            UseValueOne ? Settings.CharValueOne.Value : Settings.CharValueTwo.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.ShortValue,
            UseValueOne ? Settings.ShortValueOne.Value : Settings.ShortValueTwo.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.UShortValue,
            UseValueOne ? Settings.UShortValueOne.Value : Settings.UShortValueTwo.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.IntValue,
            UseValueOne ? Settings.IntValueOne.Value : Settings.IntValueTwo.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.UIntValue,
            UseValueOne ? Settings.UIntValueOne.Value : Settings.UIntValueTwo.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.LongValue,
            UseValueOne ? Settings.LongValueOne.Value : Settings.LongValueTwo.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.ULongValue,
            UseValueOne ? Settings.ULongValueOne.Value : Settings.ULongValueTwo.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.FloatValue,
            UseValueOne ? Settings.FloatValueOne.Value : Settings.FloatValueTwo.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.DecimalValue,
            UseValueOne ? Settings.DecimalValueOne.Value : Settings.DecimalValueTwo.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.StringValue,
            UseValueOne ? Settings.StringValueOne.Value : Settings.StringValueTwo.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.DateTimeValue,
            UseValueOne ? Settings.DateTimeValueOne.Value : Settings.DateTimeValueTwo.Value);
        SetConnectorValueIfNotEqual(ProcessDataOutputs.TimeSpanValue,
            UseValueOne ? Settings.TimeSpanValueOne.Value : Settings.TimeSpanValueTwo.Value);

        UseValueOne = !UseValueOne;
    }
}

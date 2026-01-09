using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Constant;

/// <summary>
/// Ein benutzerdefiniertes Default-Value-Attribut für Decimal-Properties, das Double-Werte
/// akzeptiert und diese in Decimal-Objekte umwandelt.
/// 
/// Dieses Attribut existiert, weil das standardmäßige DefaultValueAttribute keine Decimal-Literale
/// direkt in Attributdeklarationen akzeptieren kann (da Attribute nur Compile-Time-Konstanten
/// unterstützen und Decimal nicht als Compile-Time-Konstante gilt). Durch die Annahme eines
/// Double-Parameters und dessen Konvertierung zur Laufzeit ermöglicht dieses Attribut
/// Designer-Dateien, aussagekräftige Decimal-Standardwerte zu spezifizieren.
/// 
/// Beispiel-Verwendung: [DecimalDefaultValueAttribute(3.14d)]
/// </summary>
public class DecimalDefaultValueAttribute(double value) : DefaultValueAttribute((decimal)value);

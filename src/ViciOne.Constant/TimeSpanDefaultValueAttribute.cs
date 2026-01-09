using System;
using System.Globalization;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Constant;

/// <summary>
/// Ein benutzerdefiniertes Default-Value-Attribut für TimeSpan-Properties, das String-Literale
/// akzeptiert und diese mit der invarianten Kultur in TimeSpan-Objekte umwandelt.
/// 
/// Dieses Attribut existiert, weil das standardmäßige DefaultValueAttribute keine TimeSpan-Literale
/// direkt in Attributdeklarationen akzeptieren kann (da Attribute nur Compile-Time-Konstanten
/// unterstützen). Durch die Annahme eines String-Parameters und dessen Parsing zur Laufzeit
/// ermöglicht dieses Attribut Designer-Dateien, aussagekräftige TimeSpan-Standardwerte mit
/// Standard-Zeitspannen-Format-Strings wie "02:00:00" oder "00:45:00" zu spezifizieren.
/// 
/// Beispiel-Verwendung: [TimeSpanDefaultValueAttribute("02:00:00")]
/// </summary>
public class TimeSpanDefaultValueAttribute(string value) : DefaultValueAttribute(TimeSpan.Parse(value, CultureInfo.InvariantCulture));

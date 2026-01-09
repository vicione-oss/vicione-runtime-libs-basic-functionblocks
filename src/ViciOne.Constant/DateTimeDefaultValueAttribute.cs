using System;
using System.Globalization;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Constant;

/// <summary>
/// Ein benutzerdefiniertes Default-Value-Attribut für DateTime-Properties, das String-Literale
/// akzeptiert und diese mit der invarianten Kultur in DateTime-Objekte umwandelt.
/// 
/// Dieses Attribut existiert, weil das standardmäßige DefaultValueAttribute keine DateTime-Literale
/// direkt in Attributdeklarationen akzeptieren kann (da Attribute nur Compile-Time-Konstanten
/// unterstützen). Durch die Annahme eines String-Parameters und dessen Parsing zur Laufzeit
/// ermöglicht dieses Attribut Designer-Dateien, aussagekräftige DateTime-Standardwerte mit
/// ISO-Format-Strings wie "2025-01-01T10:00:00" zu spezifizieren.
/// 
/// Beispiel-Verwendung: [DateTimeDefaultValueAttribute("2025-01-01T10:00:00")]
/// </summary>
public class DateTimeDefaultValueAttribute(string value) : DefaultValueAttribute(DateTime.Parse(value, CultureInfo.InvariantCulture));

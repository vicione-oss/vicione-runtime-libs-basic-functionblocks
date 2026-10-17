namespace ViciOne.Analysis;

/// <summary>
///     Result of a completed interval.
/// </summary>
internal readonly record struct IntervalResult(double Average, double Minimum, double Maximum, double Sum, double Difference, double Last);

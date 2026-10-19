using System;

/// <summary>
/// Time provider with a millisecond timestamp that only moves when <see cref="Advance" /> is called.
/// </summary>
internal sealed class ManualTimeProvider : TimeProvider
{
    private long _timestamp = 1;

    public override long TimestampFrequency => 1000;

    public void Advance(int milliseconds)
        => _timestamp += milliseconds;

    public override long GetTimestamp()
        => _timestamp;
}

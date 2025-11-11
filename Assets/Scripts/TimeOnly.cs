using System;

[Serializable]
public sealed class TimeOnly
{

    public TimeSpan Value { get; set; }

    public static implicit operator TimeOnly(TimeSpan span) => new() {Value = span};

}

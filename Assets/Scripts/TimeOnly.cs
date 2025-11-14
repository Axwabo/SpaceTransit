using System;
using UnityEngine;

[Serializable]
public sealed class TimeOnly
{

    [SerializeField]
    private int hours;

    [SerializeField]
    private int minutes;

    public TimeSpan Value
    {
        get => new(hours, minutes, 0);
        set
        {
            hours = value.Hours;
            minutes = value.Minutes;
        }
    }

    public static implicit operator TimeOnly(TimeSpan span) => new() {Value = span};

}

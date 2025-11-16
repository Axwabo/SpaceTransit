using System;
using UnityEngine;

namespace SpaceTransit
{

    public static class Clock
    {

        public static TimeSpan Now => new TimeSpan(7, 29, 50).Add(TimeSpan.FromSeconds(Time.timeAsDouble));

        public static float Delta => Mathf.Min(0.3f, Time.unscaledDeltaTime) * Time.timeScale;

        public static float FixedDelta => Mathf.Min(0.3f, Time.fixedUnscaledDeltaTime) * Time.timeScale;

    }

}

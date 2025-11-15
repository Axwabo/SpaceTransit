using System;
using UnityEngine;

namespace SpaceTransit
{

    public static class Clock
    {

        public static TimeSpan Now => new TimeSpan(7, 29, 50).Add(TimeSpan.FromSeconds(Time.timeAsDouble));

    }

}

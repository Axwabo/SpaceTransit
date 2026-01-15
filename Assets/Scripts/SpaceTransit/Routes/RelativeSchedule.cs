using System;
using System.Linq;
using SpaceTransit.Routes.Stops;
using UnityEngine;

namespace SpaceTransit.Routes
{

    [CreateAssetMenu(fileName = "Relative Schedule", menuName = "SpaceTransit/Relative Schedule", order = 0)]
    public sealed class RelativeSchedule : ScriptableObject
    {

        public IntermediateStop[] intermediateStops;

        public IntermediateStop[] Map(TimeSpan departure) => intermediateStops.Select(e => e.RelativeTo(departure)).ToArray();

    }

}

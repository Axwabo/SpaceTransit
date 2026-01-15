using SpaceTransit.Routes.Stops;
using UnityEngine;

namespace SpaceTransit.Routes
{

    [CreateAssetMenu(fileName = "Relative Schedule", menuName = "SpaceTransit/Relative Schedule", order = 0)]
    public sealed class RelativeSchedule : ScriptableObject
    {

        public IntermediateStop[] intermediateStops;

    }

}

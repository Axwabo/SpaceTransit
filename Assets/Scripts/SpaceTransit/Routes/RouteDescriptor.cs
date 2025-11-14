using System.Collections.Generic;
using SpaceTransit.Routes.Stops;
using UnityEngine;

namespace SpaceTransit.Routes
{

    [CreateAssetMenu(fileName = "Route", menuName = "SpaceTransit/Route", order = 0)]
    public sealed class RouteDescriptor : ScriptableObject
    {

        [SerializeField]
        private IntermediateStop[] intermediateStops;

        [field: SerializeField]
        public bool Reverse { get; private set; }

        [field: SerializeField]
        public Origin Origin { get; private set; }

        public IReadOnlyList<IntermediateStop> IntermediateStops => intermediateStops;

        [field: SerializeField]
        public Destination Destination { get; private set; }

    }

}

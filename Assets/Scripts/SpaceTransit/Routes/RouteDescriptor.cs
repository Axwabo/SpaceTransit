using System.Collections.Generic;
using SpaceTransit.Routes.Stops;
using UnityEngine;

namespace SpaceTransit.Routes
{

    [CreateAssetMenu(fileName = "Route", menuName = "SpaceTransit/Route", order = 0)]
    public sealed class RouteDescriptor : ScriptableObject
    {

        [field: SerializeField]
        public ServiceType Type { get; private set; }

        [field: SerializeField]
        public bool Reverse { get; private set; }

        [field: SerializeField]
        public Origin Origin { get; private set; }

        [SerializeField]
        private IntermediateStop[] intermediateStops;

        public IReadOnlyList<IntermediateStop> IntermediateStops => intermediateStops;

        [field: SerializeField]
        public Destination Destination { get; private set; }

    }

}

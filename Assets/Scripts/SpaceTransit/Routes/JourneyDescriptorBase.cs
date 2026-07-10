using System;
using SpaceTransit.Routes.Stops;
using UnityEngine;

namespace SpaceTransit.Routes
{

    public abstract class JourneyDescriptorBase : ScriptableObject
    {

        [field: SerializeField]
        public bool Reverse { get; private set; }

        public abstract IExitTowards Beginning { get; }

        public abstract IDestination End { get; }

        public virtual ReadOnlySpan<IntermediateStop> IntermediateStops => default;

        public abstract ReadOnlySpan<Passthrough> Passthrough { get; }

    }

}

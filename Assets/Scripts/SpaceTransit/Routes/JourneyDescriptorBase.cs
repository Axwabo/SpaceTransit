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

        public abstract ITarget End { get; }

        public abstract ReadOnlySpan<Passthrough> Passthrough { get; }

    }

}

using System;
using UnityEngine;

namespace SpaceTransit.Routes
{

    [Serializable]
    public sealed class StopSubsetRule
    {

        [field: SerializeField]
        public StationId Start { get; private set; }

        [field: SerializeField]
        public StationId End { get; private set; }

        [field: SerializeField]
        public bool EveryStation { get; private set; }

    }

}

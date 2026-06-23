using System;
using UnityEngine;

namespace SpaceTransit.Routes.Stops
{

    [Serializable]
    public abstract class Stop : ITarget
    {

        [field: SerializeField]
        public int DockIndex { get; protected set; }

        [field: SerializeField]
        public StationId Station { get; protected set; }

    }

}

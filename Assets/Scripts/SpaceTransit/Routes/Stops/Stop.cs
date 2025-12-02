using System;
using UnityEngine;

namespace SpaceTransit.Routes.Stops
{

    [Serializable]
    public abstract class Stop : IStop
    {

        [field: SerializeField]
        public int DockIndex { get; private set; }

        [field: SerializeField]
        public StationId Station { get; private set; }

    }

}

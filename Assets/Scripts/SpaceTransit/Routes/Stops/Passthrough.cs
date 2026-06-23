using System;
using UnityEngine;

namespace SpaceTransit.Routes.Stops
{

    [Serializable]
    public sealed class Passthrough : IExitTowards
    {

        [field: SerializeField]
        public StationId AfterStop { get; private set; }

        [field: SerializeField]
        public int DockIndex { get; private set; }

        [field: SerializeField]
        public StationId Station { get; private set; }

        [field: SerializeField]
        public StationId ExitTowards { get; private set; }

    }

}

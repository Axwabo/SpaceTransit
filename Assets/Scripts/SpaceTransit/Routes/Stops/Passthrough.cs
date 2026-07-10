using System;
using UnityEngine;

namespace SpaceTransit.Routes.Stops
{

    [Serializable]
    public sealed class Passthrough : Target, IExitTowards
    {

        [field: SerializeField]
        public StationId AfterStop { get; private set; }

        [field: SerializeField]
        public StationId ExitTowards { get; private set; }

    }

}

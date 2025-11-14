using System.Collections.Generic;
using UnityEngine;

namespace SpaceTransit.Routes
{

    public sealed class Station : MonoBehaviour
    {

        [SerializeField]
        private StationId id;

        [SerializeField]
        private Dock[] docks;

        public string Name => id.name;

        public IReadOnlyList<Dock> Docks => docks;

    }

}

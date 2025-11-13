using System.Collections.Generic;
using UnityEngine;

namespace SpaceTransit.Routes
{

    public sealed class Station : MonoBehaviour
    {

        [SerializeField]
        private Dock[] docks;

        public IReadOnlyList<Dock> Docks => docks;

    }

}

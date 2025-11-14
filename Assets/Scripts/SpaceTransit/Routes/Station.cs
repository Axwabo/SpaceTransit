using System.Collections.Generic;
using UnityEngine;

namespace SpaceTransit.Routes
{

    public sealed class Station : MonoBehaviour
    {

        private static readonly Dictionary<string, Station> Loaded = new();

        public static bool TryGetLoadedStation(StationId id, out Station station)
            => Loaded.TryGetValue(id.name, out station);

        [SerializeField]
        private StationId id;

        [SerializeField]
        private Dock[] docks;

        public string Name => id.name;

        public IReadOnlyList<Dock> Docks => docks;

        private void OnEnable() => Loaded[Name] = this;

        private void OnDisable() => Loaded.Remove(Name);

    }

}

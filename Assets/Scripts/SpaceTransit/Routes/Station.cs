using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpaceTransit.Routes
{

    public sealed class Station : MonoBehaviour
    {

        private static readonly Dictionary<string, Station> Loaded = new();

        public static bool TryGetLoadedStation(StationId id, out Station station)
            => Loaded.TryGetValue(id.name, out station);

        [field: SerializeField]
        [field: FormerlySerializedAs("id")]
        public StationId ID { get; private set; }

        [SerializeField]
        private Dock[] docks;

        public string Name => ID.name;

        public IReadOnlyList<Dock> Docks => docks;

        private void OnEnable() => Loaded[Name] = this;

        private void OnDisable() => Loaded.Remove(Name);

    }

}

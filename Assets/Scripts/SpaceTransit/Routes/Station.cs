using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpaceTransit.Routes
{

    public sealed class Station : MonoBehaviour
    {

        private static readonly Dictionary<string, Station> Loaded = new();

        public static IReadOnlyCollection<Station> LoadedStations => Loaded.Values;

        public static bool TryGetLoadedStation(StationId id, out Station station)
            => Loaded.TryGetValue(id.name, out station);

        [field: SerializeField]
        [field: FormerlySerializedAs("id")]
        public StationId ID { get; private set; }

        [SerializeField]
        private Dock[] docks;

        [SerializeField]
        private Vector3 spawnpoint;

        public Vector3 Spawnpoint => transform.TransformPoint(spawnpoint);

        public string Name => ID.name;

        public ReadOnlySpan<Dock> Docks => docks;

        private void OnEnable() => Loaded[Name] = this;

        private void Start()
        {
            for (var i = 0; i < docks.Length; i++)
            {
                var dock = docks[i];
                dock.Station = this;
                dock.Index = i;
            }
        }

        private void OnDisable() => Loaded.Remove(Name);

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(Spawnpoint, 0.5f);
        }

    }

}

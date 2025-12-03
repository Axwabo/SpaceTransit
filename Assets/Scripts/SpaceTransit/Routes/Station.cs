using System.Collections.Generic;
using SpaceTransit.Movement;
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
        private GameObject active;

        [SerializeField]
        private Vector3 spawnpoint;

        public Vector3 Spawnpoint => transform.TransformPoint(spawnpoint);

        private bool _previouslyActive;

        public string Name => ID.name;

        public IReadOnlyList<Dock> Docks => docks;

        private void OnEnable() => Loaded[Name] = this;

        private void OnDisable() => Loaded.Remove(Name);

        private void Update()
        {
            var activate = Vector3.Distance(MovementController.Current.LastPosition, transform.position) < 10;
            if (_previouslyActive == activate)
                return;
            _previouslyActive = activate;
            active.SetActive(activate);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(Spawnpoint, 0.5f);
        }

    }

}

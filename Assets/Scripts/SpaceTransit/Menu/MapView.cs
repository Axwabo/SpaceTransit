using System.Collections.Generic;
using SpaceTransit.Routes;
using SpaceTransit.Ships;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace SpaceTransit.Menu
{

    [RequireComponent(typeof(RectTransform))]
    public sealed class MapView : MonoBehaviour
    {

        private static Vector2 Mouse => InputSystem.actions["Point"].ReadValue<Vector2>();

        [FormerlySerializedAs("prefab")]
        [SerializeField]
        private MapStation stationPrefab;

        [SerializeField]
        private MapShip shipPrefab;

        [SerializeField]
        private RectTransform constraints;

        [SerializeField]
        private Transform anchor;

        [SerializeField]
        private float scale = 1;

        [SerializeField]
        private float sensitivity = 1;

        private bool _placed;

        private RectTransform _this;

        private Vector2 _previousPoint;

        private readonly HashSet<ShipAssembly> _spawnedAssemblies = new();

        private void Awake() => _this = (RectTransform) transform;

        private void OnEnable()
        {
            _this.anchoredPosition = Vector2.zero;
            _previousPoint = Mouse;
        }

        private void Start()
        {
            if (_placed)
                return;
            foreach (var station in Station.LoadedStations)
            {
                var stationPosition = anchor.InverseTransformPoint(station.transform.localPosition);
                Instantiate(stationPrefab, _this, false).Apply(station, new Vector3(stationPosition.x * scale, stationPosition.z * scale));
            }
        }

        private void Update()
        {
            foreach (var assembly in ShipAssembly.Instances)
            {
                if (!_spawnedAssemblies.Add(assembly))
                    continue;
                var ship = Instantiate(shipPrefab, _this, false);
                ship.Scale = scale;
                ship.Apply(anchor, assembly);
            }

            var point = Mouse;
            if (!InputSystem.actions["Click"].IsPressed())
            {
                _previousPoint = Mouse;
                return;
            }

            if (point == _previousPoint)
                return;
            _this.anchoredPosition += (point - _previousPoint) * sensitivity;
            _previousPoint = point;
        }

    }

}

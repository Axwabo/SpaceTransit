using System.Collections.Generic;
using SpaceTransit.Routes;
using SpaceTransit.Ships;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceTransit.Menu
{

    [RequireComponent(typeof(RectTransform))]
    public sealed class MapView : MonoBehaviour
    {

        private static Vector2 Mouse => InputSystem.actions["Point"].ReadValue<Vector2>();

        [SerializeField]
        private MapStation stationPrefab;

        [SerializeField]
        private MapShip shipPrefab;

        [SerializeField]
        private Transform anchor;

        [SerializeField]
        private float scale = 1;

        [SerializeField]
        private float sensitivity = 1;

        [SerializeField]
        private float scrollSensitivity = 0.1f;

        private bool _placed;

        private RectTransform _this;

        private Vector2 _previousPoint;

        private Vector2 _size;

        private float _zoom = 1;

        private readonly HashSet<ShipAssembly> _spawnedAssemblies = new();

        private void Awake()
        {
            _this = (RectTransform) transform;
            _size = _this.sizeDelta;
        }

        private void OnEnable() => _previousPoint = Mouse;

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

            var scroll = InputSystem.actions["Speed"].ReadValue<float>();
            if (scroll != 0)
            {
                _zoom = Mathf.Clamp(_zoom + scroll * scrollSensitivity, 0.1f, 10);
                _this.localScale = Vector3.one * _zoom;
            }

            var point = Mouse;
            if (!InputSystem.actions["Click"].IsPressed())
            {
                _previousPoint = point;
                return;
            }

            if (point == _previousPoint)
                return;
            var delta = _previousPoint - point;
            _this.pivot += new Vector2(delta.x * sensitivity / _size.x / _zoom, delta.y * sensitivity / _size.y / _zoom);
            _previousPoint = point;
        }

    }

}

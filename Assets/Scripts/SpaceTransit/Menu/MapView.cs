using System.Collections.Generic;
using SpaceTransit.Loader;
using SpaceTransit.Movement;
using SpaceTransit.Routes;
using SpaceTransit.Ships;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace SpaceTransit.Menu
{

    public sealed class MapView : MonoBehaviour
    {

        private static Vector2 Mouse => InputSystem.actions["Point"].ReadValue<Vector2>();

        [SerializeField]
        private VisualTreeAsset stationPrefab;

        [SerializeField]
        private VisualTreeAsset shipPrefab;

        [SerializeField]
        private Transform anchor;

        [SerializeField]
        private float scale = 1;

        [SerializeField]
        private float sensitivity = 1;

        [SerializeField]
        private float scrollSensitivity = 0.1f;

        private VisualElement _items;

        private bool _placed;

        private float _zoom = 1;

        private readonly HashSet<ShipAssembly> _spawnedAssemblies = new();

        private readonly HashSet<StationId> _placedStations = new();

        private void Awake() => WorldChanger.SceneFullyLoaded += OnSceneLoaded;

        private void Start()
        {
            var root = this.RootVisual();
            _items = root.Q("Items");
            var start = GetAnchored(World.Current.InverseTransformPoint(MovementController.Current.Position));
            _items.style.transformOrigin = new TransformOrigin(start.x, start.y);
            Place();
        }

        private void Place()
        {
            _placed = true;
            foreach (var station in Station.LoadedStations)
                if (_placedStations.Add(station.ID))
                {
                    var anchored = GetAnchored(station.transform.position);
                    var element = stationPrefab.Instantiate();
                    element.AddToClassList("map-item");
                    element.style.top = anchored.y;
                    element.style.left = anchored.x;
                    element.Q<Label>().text = station.name;
                    _items.Add(element);
                }
        }

        private void Update()
        {
            if (!_placed)
                Place();
            // TODO
            /*foreach (var assembly in ShipAssembly.Instances)
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
            _previousPoint = point;*/
        }

        private void OnDestroy() => WorldChanger.SceneFullyLoaded -= OnSceneLoaded;

        private Vector2 GetAnchored(Vector3 localPosition)
        {
            var anchored = anchor.InverseTransformPoint(localPosition);
            return new Vector2(anchored.x * scale, -anchored.z * scale);
        }

        private void OnSceneLoaded() => _placed = false;

    }

}

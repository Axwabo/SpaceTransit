using System.Collections.Generic;
using SpaceTransit.Loader;
using SpaceTransit.Movement;
using SpaceTransit.Routes;
using SpaceTransit.Ships;
using UnityEngine;
using UnityEngine.UIElements;
using Cache = SpaceTransit.Vaulter.Cache;

namespace SpaceTransit.Menu
{

    public sealed class MapView : MonoBehaviour
    {

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

        private VisualElement _anchor;

        private VisualElement _items;

        private bool _placed;

        private float _zoom = 1;

        private bool _pointerDown;

        private readonly List<MapShip> _ships = new();

        private readonly HashSet<ShipAssembly> _spawnedAssemblies = new();

        private readonly HashSet<StationId> _placedStations = new();

        private void Awake() => WorldChanger.SceneFullyLoaded += OnSceneLoaded;

        private void Start()
        {
            var root = this.RootVisual();
            var container = root.Q("Container");
            _anchor = root.Q("Anchor");
            _items = root.Q("Items");
            _items.style.translate = -GetAnchored(World.Current.InverseTransformPoint(MovementController.Current.Position));
            container.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            container.RegisterCallback<WheelEvent>(OnWheel);
            Place();
        }

        private void OnWheel(WheelEvent evt)
        {
            var scroll = evt.delta.y switch
            {
                > 0 => -1,
                < 0 => 1,
                _ => 0
            };
            if (scroll == 0)
                return;
            _zoom = Mathf.Clamp(_zoom + scroll * scrollSensitivity, 0.1f, 10);
            _anchor.style.scale = Vector3.one * _zoom;
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            if (evt.pressedButtons != 1)
                return;
            var currentTranslate = _items.style.translate.value;
            var scalar = sensitivity / Mathf.Pow(scale, 0.75f);
            _items.style.translate = new Vector2(currentTranslate.x.value + evt.deltaPosition.x * scalar, currentTranslate.y.value + evt.deltaPosition.y * scalar);
        }

        private void Place()
        {
            _placed = true;
            foreach (var station in Cache.Stations)
                if (station.position != Vector3.zero && _placedStations.Add(station))
                    PlaceStation(station.position, station.name);
            foreach (var station in Station.LoadedStations)
                if (_placedStations.Add(station.ID))
                    PlaceStation(station.transform.localPosition, station.ID.name);
        }

        private void PlaceStation(Vector3 position, string stationName)
        {
            var element = CreateMapItem(stationPrefab, position);
            element.Q<Label>().text = stationName;
            _items.Add(element);
        }

        private TemplateContainer CreateMapItem(VisualTreeAsset prefab, Vector3 position)
        {
            var anchored = GetAnchored(position);
            var element = prefab.Instantiate();
            element.AddToClassList("map-item");
            element.style.translate = anchored;
            return element;
        }

        private void Update()
        {
            if (!_placed)
                Place();
            foreach (var assembly in ShipAssembly.Instances)
            {
                if (!_spawnedAssemblies.Add(assembly))
                    continue;
                var element = CreateMapItem(shipPrefab, Vector3.zero);
                _ships.Add(new MapShip(element, assembly, this) {Scale = scale});
                _items.Add(element);
            }

            for (var i = _ships.Count - 1; i >= 0; i--)
            {
                var ship = _ships[i];
                if (ship.Update())
                    continue;
                ship.Element.RemoveFromHierarchy();
                _ships.RemoveAt(i);
                _spawnedAssemblies.Remove(ship.Assembly);
            }
        }

        private void OnDestroy() => WorldChanger.SceneFullyLoaded -= OnSceneLoaded;

        public Vector2 GetAnchored(Vector3 localPosition)
        {
            var anchored = anchor.InverseTransformPoint(localPosition);
            return new Vector2(anchored.x * scale, -anchored.z * scale);
        }

        private void OnSceneLoaded() => _placed = false;

    }

}

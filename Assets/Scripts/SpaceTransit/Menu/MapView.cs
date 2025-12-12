using SpaceTransit.Routes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceTransit.Menu
{

    [RequireComponent(typeof(RectTransform))]
    public sealed class MapView : MonoBehaviour
    {

        private static Vector2 Mouse => InputSystem.actions["Point"].ReadValue<Vector2>();

        [SerializeField]
        private MapStation prefab;

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
            var parent = _this;
            foreach (var station in Station.LoadedStations)
            {
                var t = new GameObject(station.Name).transform;
                var stationPosition = anchor.InverseTransformPoint(station.transform.localPosition);
                t.parent = constraints;
                t.localPosition = new Vector3(stationPosition.x * scale, stationPosition.z * scale);
                Instantiate(prefab, parent, false).Apply(station, t);
            }
        }

        private void Update()
        {
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

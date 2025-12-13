using SpaceTransit.Routes;
using SpaceTransit.Ships;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceTransit.Menu
{

    [RequireComponent(typeof(RectTransform))]
    public sealed class MapShip : MonoBehaviour
    {

        [SerializeField]
        private TextMeshProUGUI route;

        [SerializeField]
        private TextMeshProUGUI type;

        [SerializeField]
        private RectTransform point;

        [SerializeField]
        private Image image;

        private ShipAssembly _assembly;

        private Transform _anchor;

        private RectTransform _this;

        private RouteDescriptor _previousRoute;

        public float Scale { get; set; } = 1;

        private void Awake() => _this = (RectTransform) transform;

        public void Apply(Transform anchor, ShipAssembly assembly)
        {
            _assembly = assembly;
            _anchor = anchor;
            UpdatePosition();
            if (!assembly.Parent.TryGetVaulter(out var vaulter))
                return;
            var currentRoute = vaulter.Route;
            Apply(currentRoute);
        }

        private void Apply(RouteDescriptor currentRoute)
        {
            _previousRoute = currentRoute;
            route.text = currentRoute?.name ?? "---";
            (type.text, image.color) = currentRoute?.Type switch
            {
                ServiceType.Fast => ("F", Color.orangeRed),
                ServiceType.InterHub => ("IH", Color.darkBlue),
                ServiceType.Passenger => ("P", Color.deepSkyBlue),
                _ => ("-", Color.gray)
            };
        }

        private void Update() => UpdatePosition();

        private void UpdatePosition()
        {
            _assembly.FrontModule.Transform.GetLocalPositionAndRotation(out var pos, out var rot);
            var position = _anchor.InverseTransformPoint(pos);
            var rotation = rot.eulerAngles.y;
            if (_assembly.Reverse)
                rotation += 180;
            _this.anchoredPosition = new Vector2(position.x * Scale, position.z * Scale);
            point.eulerAngles = new Vector3(0, 0, -rotation);
            if (!_assembly.Parent.TryGetVaulter(out var vaulter))
                return;
            var currentRoute = vaulter.Route;
            if (currentRoute != _previousRoute)
                Apply(currentRoute);
        }

    }

}

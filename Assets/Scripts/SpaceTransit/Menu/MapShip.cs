using System.Threading;
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

        private CancellationToken _cancellationToken;

        public float Scale { get; set; } = 1;

        private void Awake() => _this = (RectTransform) transform;

        public void Apply(Transform anchor, ShipAssembly assembly)
        {
            _assembly = assembly;
            _anchor = anchor;
            _cancellationToken = assembly.destroyCancellationToken;
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
            (type.text, image.color) = currentRoute.GetAbbreviation();
        }

        private void Update()
        {
            if (_cancellationToken.IsCancellationRequested)
                Destroy(gameObject);
            else
                UpdatePosition();
        }

        private void UpdatePosition()
        {
            _assembly.FrontModule.Transform.GetLocalPositionAndRotation(out var pos, out var rot);
            var position = _anchor.InverseTransformPoint(pos);
            var rotation = rot.eulerAngles.y;
            if (_assembly.Reverse)
                rotation += 180;
            _this.anchoredPosition = new Vector2(position.x * Scale, position.z * Scale);
            point.eulerAngles = new Vector3(0, 0, -rotation);
            if (!_assembly.didStart || !_assembly.Parent.TryGetVaulter(out var vaulter))
                return;
            var currentRoute = vaulter.Route;
            if (currentRoute != _previousRoute)
                Apply(currentRoute);
        }

    }

}

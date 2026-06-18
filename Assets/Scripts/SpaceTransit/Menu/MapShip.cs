using System.Threading;
using SpaceTransit.Routes;
using SpaceTransit.Ships;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Menu
{

    public sealed class MapShip
    {

        private readonly MapView _view;

        private readonly CancellationToken _cancellationToken;

        public VisualElement Element { get; }

        public ShipAssembly Assembly { get; }

        private readonly Label _type;

        private readonly Label _route;

        private readonly VisualElement _image;

        private RouteDescriptor _previousRoute;

        public MapShip(VisualElement element, ShipAssembly assembly, MapView view)
        {
            _view = view;
            Element = element;
            Assembly = assembly;
            _type = Element.Q<Label>("Type");
            _route = Element.Q<Label>("Route");
            _image = Element.Q<VisualElement>("Background");
            _cancellationToken = assembly.destroyCancellationToken;
        }

        public float Scale { get; init; } = 1;

        private void Apply(RouteDescriptor currentRoute)
        {
            _previousRoute = currentRoute;
            _route.text = currentRoute?.name ?? "---";
            (_type.text, _image.style.unityBackgroundImageTintColor) = currentRoute.GetAbbreviation();
        }

        public bool Update()
        {
            if (_cancellationToken.IsCancellationRequested)
                return false;
            UpdatePosition();
            return true;
        }

        private void UpdatePosition()
        {
            Assembly.FrontModule.Transform.GetLocalPositionAndRotation(out var pos, out var rot);
            var rotation = rot.eulerAngles.y;
            if (Assembly.Reverse)
                rotation += 180;
            Element.style.translate = _view.GetAnchored(pos);
            _image.style.rotate = new Rotate(rotation, Vector3.forward);
            if (!Assembly.didStart || !Assembly.Parent.TryGetVaulter(out var vaulter))
                return;
            var currentRoute = vaulter.Route;
            if (currentRoute != _previousRoute)
                Apply(currentRoute);
        }

    }

}

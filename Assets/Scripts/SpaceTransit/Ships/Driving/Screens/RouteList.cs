using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Driving.Screens
{

    [RequireComponent(typeof(RouteListManager))]
    public sealed class RouteList : PickableList<RoutePicker>
    {

        private RouteListManager _manager;

        private void Awake() => _manager = GetComponent<RouteListManager>();

        protected override string GetContent(RoutePicker item)
            => ReferenceEquals(item, RoutePicker.ExitService) ? "Exit Service" : Menu.RouteList.Format(item.Descriptor);

        protected override void Select(RoutePicker item)
        {
            if (!_manager.Controller.TryGetVaulter(out var vaulter))
                return;
            if (ReferenceEquals(item, RoutePicker.ExitService))
                vaulter.ExitService();
            else
                vaulter.BeginRoute(item.Descriptor);
        }

        public override void Navigate(bool forwards)
        {
            base.Navigate(forwards);
            List.ScrollToItem(Selected);
        }

        protected override ListView GetListView(VisualElement root) => root.Q<ListView>("Routes");

    }

}

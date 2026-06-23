using System.Collections.Generic;
using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Ships.Driving.Screens;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Menu
{

    public sealed class RouteTimetable : MonoBehaviour
    {

        private readonly List<ITarget> _stops = new();

        private Label _type;

        private Label _summary;

        private ListView _list;

        private void Start()
        {
            var root = this.RootVisual();
            _type = root.Q<Label>("Type");
            _summary = root.Q<Label>("Summary");
            _list = root.Q<ListView>("Stops");
            _list.itemsSource = _stops;
            _list.bindItem = StopList.CreateBindFunction(_stops);
        }

        public void Apply(RouteDescriptor descriptor)
        {
            _type.text = $"{descriptor.name} {descriptor.Type.ToStringFast()}";
            _summary.text = descriptor.Summary();
            _stops.Clear();
            _stops.Add(descriptor.Origin);
            foreach (var stop in descriptor.IntermediateStops)
                _stops.Add(stop);
            _stops.Add(descriptor.Destination);
            _list.RefreshItems();
        }

    }

}

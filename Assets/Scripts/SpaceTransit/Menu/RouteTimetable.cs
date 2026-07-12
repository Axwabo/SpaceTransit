using System.Collections.Generic;
using System.Linq;
using SpaceTransit.Routes;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;
using Cache = SpaceTransit.Vaulter.Cache;

namespace SpaceTransit.Menu
{

    public sealed class RouteTimetable : MonoBehaviour
    {

        private int _selected = -1;

        private RouteDescriptor[] _routes;

        [CreateProperty]
        public string[] Routes { get; set; }

        [CreateProperty]
        public int SelectedIndex
        {
            get => _selected;
            set
            {
                _selected = value;
                if (value != -1)
                    Apply(_routes[value]);
            }
        }

        [CreateProperty]
        public string Type { get; private set; }

        [CreateProperty]
        public string Summary { get; private set; }

        [CreateProperty]
        // ReSharper disable once CollectionNeverQueried.Global
        public List<RouteStopItem> Stops { get; } = new();

        private ListView _list;

        private void Start()
        {
            var root = this.RootVisual();
            _list = root.Q<ListView>("Stops");
            _routes = Cache.Journeys.OfType<RouteDescriptor>().OrderBy(e => int.Parse(e.name)).ToArray();
            Routes = _routes.Select(e => $"{e.name} {e.Origin.Station.name} - {e.Destination.Station.name}").ToArray();
            root.dataSource = this;
        }

        private void Apply(RouteDescriptor descriptor)
        {
            Type = $"{descriptor.name} {descriptor.Type.ToStringFast()}";
            Summary = descriptor.Summary();
            Stops.Clear();
            Stops.Add(descriptor.Origin);
            foreach (var stop in descriptor.IntermediateStops)
                Stops.Add(stop);
            Stops.Add(descriptor.Destination);
            _list.RefreshItems();
        }

    }

}

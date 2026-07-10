using System.Collections.Generic;
using System.Linq;
using SpaceTransit.Routes;
using UnityEngine;
using UnityEngine.UIElements;
using Cache = SpaceTransit.Vaulter.Cache;

namespace SpaceTransit.Menu
{

    public sealed class RouteList : MonoBehaviour
    {

        private ListView _list;

        private RouteDescriptor[] _routes;

        [SerializeField]
        private RouteTimetable timetable;

        private void Start()
        {
            _routes = Cache.Journeys.OfType<RouteDescriptor>().OrderBy(e => int.Parse(e.name)).ToArray();
            _list = this.RootVisual().Q<ListView>("Routes");
            _list.makeItem = () => new Label();
            _list.bindItem = Bind;
            _list.itemsSource = _routes;
            _list.selectionChanged += OnSelectionChanged;
        }

        private void Bind(VisualElement element, int i)
        {
            var route = _routes[i];
            var label = element as Label ?? element.Q<Label>();
            label.text = $"{route.name} {route.Beginning.Station.name} - {route.End.Station.name}";
        }

        private void OnSelectionChanged(IEnumerable<object> objects)
        {
            var descriptor = objects.FirstOrDefault();
            if (descriptor is RouteDescriptor route)
                timetable.Apply(route);
        }

    }

}

using System.Linq;
using SpaceTransit.Routes;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;
using Cache = SpaceTransit.Vaulter.Cache;

namespace SpaceTransit.Menu
{

    public sealed class RouteList : MonoBehaviour
    {

        private int _selected = -1;

        private ListView _list;

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
                    timetable.Apply(_routes[value]);
            }
        }

        [SerializeField]
        private RouteTimetable timetable;

        private void Start()
        {
            _routes = Cache.Journeys.OfType<RouteDescriptor>().OrderBy(e => int.Parse(e.name)).ToArray();
            Routes = _routes.Select(e => $"{e.name} {e.Origin.Station.name} - {e.Destination.Station.name}").ToArray();
            this.RootVisual().dataSource = this;
        }

    }

}

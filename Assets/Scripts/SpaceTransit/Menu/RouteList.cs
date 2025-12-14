using System.Linq;
using SpaceTransit.Routes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cache = SpaceTransit.Vaulter.Cache;

namespace SpaceTransit.Menu
{

    public sealed class RouteList : MonoBehaviour
    {

        [SerializeField]
        private GameObject prefab;

        [SerializeField]
        private RouteTimetable timetable;

        private void Start()
        {
            var t = transform;
            foreach (var route in Cache.Routes.OrderBy(static e => int.Parse(e.name)))
            {
                var picker = Instantiate(prefab, t).AddComponent<RoutePicker>();
                picker.Route = route;
                picker.Timetable = timetable;
            }
        }

        // TODO: refactor
        private sealed class RoutePicker : MonoBehaviour
        {

            public RouteDescriptor Route { get; set; }

            public RouteTimetable Timetable { get; set; }

            private void Start()
            {
                GetComponent<Button>().onClick.AddListener(Click);
                GetComponentInChildren<TextMeshProUGUI>().text = $"{Route.name} {Route.Origin.Station.name} - {Route.Destination.Station.name}";
            }

            private void Click() => Timetable.Apply(Route);

        }

    }

}

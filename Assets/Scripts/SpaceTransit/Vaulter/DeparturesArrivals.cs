using System.Collections.Generic;
using SpaceTransit.Routes;
using UnityEngine;

namespace SpaceTransit.Vaulter
{

    [RequireComponent(typeof(Station))]
    public sealed class DeparturesArrivals : MonoBehaviour
    {

        public IReadOnlyList<DepartureEntry> Departures { get; private set; }

        public IReadOnlyList<ArrivalEntry> Arrivals { get; private set; }

        private void Awake()
        {
            var departures = new List<DepartureEntry>();
            var arrivals = new List<ArrivalEntry>();
            var id = GetComponent<Station>().ID;
            foreach (var route in Cache.Routes)
            {
                if (route.Origin.Station.Name == id.Name)
                {
                    departures.Add(new DepartureEntry(route, -1, route.Origin));
                    continue;
                }

                if (route.Destination.Station.Name == id.Name)
                {
                    arrivals.Add(new ArrivalEntry(route, -1, route.Destination));
                    continue;
                }

                AddIntermediateStops(route, id, departures, arrivals);
            }

            Departures = departures;
            Arrivals = arrivals;
        }

        private static void AddIntermediateStops(RouteDescriptor route, StationId id, List<DepartureEntry> departures, List<ArrivalEntry> arrivals)
        {
            for (var i = 0; i < route.IntermediateStops.Count; i++)
            {
                var stop = route.IntermediateStops[i];
                if (stop.Station.Name != id.Name)
                    break;
                departures.Add(new DepartureEntry(route, i, stop));
                arrivals.Add(new ArrivalEntry(route, i, stop));
            }
        }

    }

}

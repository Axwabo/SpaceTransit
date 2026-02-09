using System.Collections.Generic;
using SpaceTransit.Routes;
using UnityEngine;

namespace SpaceTransit.Vaulter
{

    [RequireComponent(typeof(Station))]
    public sealed class DeparturesArrivals : MonoBehaviour
    {

        public List<DepartureEntry> Departures { get; private set; }

        public List<ArrivalEntry> Arrivals { get; private set; }

        public Station Station { get; private set; }
        
        public StationId StationId { get; private set; }

        private void Awake()
        {
            Station = GetComponent<Station>();
            StationId = Station.ID;
            var departures = new List<DepartureEntry>();
            var arrivals = new List<ArrivalEntry>();
            foreach (var route in Cache.Routes)
            {
                if (route.Origin.Station.name == StationId.name)
                {
                    departures.Add(new DepartureEntry(route, -1, route.Origin));
                    continue;
                }

                if (route.Destination.Station.name == StationId.name)
                {
                    arrivals.Add(new ArrivalEntry(route, -1, route.Destination));
                    continue;
                }

                AddIntermediateStops(route, StationId, departures, arrivals);
            }

            departures.Sort(static (a, b) => a.Departure.Departure.Value.CompareTo(b.Departure.Departure.Value));
            arrivals.Sort(static (a, b) => a.Arrival.Arrival.Value.CompareTo(b.Arrival.Arrival.Value));
            Departures = departures;
            Arrivals = arrivals;
        }

        private static void AddIntermediateStops(RouteDescriptor route, StationId id, List<DepartureEntry> departures, List<ArrivalEntry> arrivals)
        {
            for (var i = 0; i < route.IntermediateStops.Length; i++)
            {
                var stop = route.IntermediateStops[i];
                if (stop.Station.name != id.name)
                    continue;
                departures.Add(new DepartureEntry(route, i, stop));
                arrivals.Add(new ArrivalEntry(route, i, stop));
            }
        }

    }

}

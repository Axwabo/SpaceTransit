using System.Collections.Generic;
using SpaceTransit.Vaulter;
using UnityEngine.UIElements;

namespace SpaceTransit.Stations
{

    public sealed class DeparturesBoard : Board<DepartureEntry>
    {

        protected override string ClassName => "departures";

        protected override List<DepartureEntry> GetSource(DeparturesArrivals departuresArrivals) => departuresArrivals.Departures;

        protected override TimeOnly GetTime(DepartureEntry entry) => entry.Departure.Departure;

        protected override void Bind(VisualElement element, DepartureEntry entry) => Apply(
            element,
            entry.Route.Type,
            entry.Route.Origin.Station.name,
            entry.Departure.Departure.Value,
            entry.Departure.DockIndex
        );

    }

}

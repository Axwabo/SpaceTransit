using System.Collections.Generic;
using SpaceTransit.Vaulter;
using UnityEngine.UIElements;

namespace SpaceTransit.Stations
{

    public sealed class ArrivalsBoard : Board<ArrivalEntry>
    {

        protected override string ClassName => "arrivals";

        protected override List<ArrivalEntry> GetSource(DeparturesArrivals departuresArrivals) => departuresArrivals.Arrivals;

        protected override TimeOnly GetTime(ArrivalEntry entry) => entry.Arrival.Arrival;

        protected override void Bind(VisualElement element, ArrivalEntry entry) => Apply(
            element,
            entry.Route.Type,
            entry.Route.Origin.Station.name,
            entry.Arrival.Arrival.Value,
            entry.Arrival.DockIndex
        );

    }

}

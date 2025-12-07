using System.Collections.Generic;
using SpaceTransit.Vaulter;

namespace SpaceTransit.Stations
{

    public sealed class ArrivalsBoard : Board<ArrivalEntry, ArrivalDisplay>
    {

        protected override IReadOnlyCollection<ArrivalEntry> GetSource(DeparturesArrivals departuresArrivals) => departuresArrivals.Arrivals;

        protected override TimeOnly GetTime(ArrivalEntry entry) => entry.Arrival.Arrival;

    }

}

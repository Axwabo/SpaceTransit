using System.Collections.Generic;
using SpaceTransit.Vaulter;

namespace SpaceTransit.Stations
{

    public sealed class DeparturesBoard : Board<DepartureEntry, DepartureDisplay>
    {

        protected override IReadOnlyCollection<DepartureEntry> GetSource(DeparturesArrivals departuresArrivals) => departuresArrivals.Departures;

        protected override TimeOnly GetTime(DepartureEntry entry) => entry.Departure.Departure;

    }

}

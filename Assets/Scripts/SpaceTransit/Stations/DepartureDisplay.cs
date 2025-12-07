using SpaceTransit.Vaulter;

namespace SpaceTransit.Stations
{

    public sealed class DepartureDisplay : EntryDisplayBase<DepartureEntry>
    {

        public override void Apply(DepartureEntry item) => Apply(
            item.Route.Type,
            item.Route.Destination.Station.name,
            item.Departure.Departure.Value,
            item.Departure.DockIndex
        );

    }

}

using SpaceTransit.Vaulter;

namespace SpaceTransit.Stations
{

    public sealed class ArrivalDisplay : EntryDisplayBase<ArrivalEntry>
    {

        public override void Apply(ArrivalEntry item) => Apply(
            item.Route.Type,
            item.Route.Destination.Station.name,
            item.Arrival.Arrival.Value,
            item.Arrival.DockIndex
        );

    }

}

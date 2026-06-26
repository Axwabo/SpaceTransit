using SpaceTransit.Routes.Stops;
using static SpaceTransit.Stations.Announcements.AnnouncementCreator;

namespace SpaceTransit.Stations.Announcements
{

    public sealed class DefaultKatilect : IKatilect
    {

        private static string DepartingCore(AnnouncementContext<IDeparture> context)
            => $"The {context.Type} ship to {context.Destination} is departing from dock {context.Dock}";

        public string DepartsFor(ref AnnouncementContext<IDeparture> context, int stopIndex)
            => Departs(context, stopIndex);

        public string DepartingIn(ref AnnouncementContext<IDeparture> context, int minutes)
            => $"{DepartingCore(context)} in {minutes} minutes. {PleaseBoard}";

        public string DepartingImmediately(ref AnnouncementContext<IDeparture> context)
            => $"{DepartingCore(context)} immediately. Please stop boarding.";

        public string Departing(ref AnnouncementContext<IDeparture> context)
            => $"{DepartingCore(context)}. {PleaseBoard}";

        public string ArrivingAndDepartsFor(ref AnnouncementContext<IArrival> context, int stopIndex)
            => ArrivingAndDeparts(context, stopIndex);

        public string Arriving(ref AnnouncementContext<IArrival> context)
            => $"{context.Type} ship is arriving from {context.Origin} at dock {context.Dock}.";

    }

}

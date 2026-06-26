using SpaceTransit.Routes.Stops;

namespace SpaceTransit.Stations.Announcements.Katilects
{

    public interface IKatilect
    {

        public static IKatilect Default { get; } = new DefaultKatilect();

        string DepartsFor(ref AnnouncementContext<IDeparture> context, int stopIndex);

        string DepartingIn(ref AnnouncementContext<IDeparture> context, int minutes);

        string DepartingImmediately(ref AnnouncementContext<IDeparture> context);

        string Departing(ref AnnouncementContext<IDeparture> context);

        string ArrivingAndDepartsFor(ref AnnouncementContext<IArrival> context, int stopIndex);

        string Arriving(ref AnnouncementContext<IArrival> context);

    }

}

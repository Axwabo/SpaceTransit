using System.Text;
using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Ships;

namespace SpaceTransit.Stations.Announcements
{

    public static class AnnouncementCreator
    {

        public const string PleaseBoard = "Please board the ship.";

        private static bool AnyShipWithStop(IStop stop)
        {
            foreach (var assembly in ShipAssembly.Instances)
                if (assembly.Parent.TryGetVaulter(out var vaulter) && vaulter.Stop == stop)
                    return true;
            return false;
        }

        public static string GetAnnouncement(this IKatilect katilect, ref AnnouncementContext<IDeparture> context, int index, int lastAnnounced)
        {
            if ((int) Clock.Now.TotalMinutes == lastAnnounced || index != -1)
                return null;
            var remaining = context.Stop.MinutesToDeparture();
            return remaining switch
            {
                3 or 5 => katilect.DepartingIn(ref context, remaining),
                1 => katilect.DepartingImmediately(ref context),
                <= 15 when lastAnnounced == -1 && AnyShipWithStop(context.Stop) => katilect.DepartsFor(ref context, index),
                _ => null
            };
        }

        public static string GetAnnouncement(this IKatilect katilect, ref AnnouncementContext<IArrival> context, int index, int lastAnnounced)
            => (int) Clock.Now.TotalMinutes == lastAnnounced || context.Stop.MinutesToArrival() is not (1 or 2)
                ? null
                : index != -1
                    ? katilect.ArrivingAndDepartsFor(ref context, index)
                    : AnyShipWithStop(context.Stop)
                        ? katilect.Arriving(ref context)
                        : null;

        public static string ArrivingAndDeparts(AnnouncementContext<IArrival> context, int index)
        {
            var sb = new StringBuilder()
                .Append(context.Type)
                .Append(" ship is arriving from ")
                .Append(context.Origin)
                .Append(" at dock ")
                .Append(context.Dock)
                .Append(" and departs for ")
                .Append(context.Destination)
                .Append('.');
            AppendIntermediateStops(context.Route, index, sb);
            return sb.ToString();
        }

        public static void AppendIntermediateStops(RouteDescriptor route, int index, StringBuilder sb)
        {
            var intermediateStops = route.IntermediateStops.Length;
            if (index == intermediateStops - 1)
                return;
            sb.Append(" The ship stops ");
            if (route.EveryStation)
            {
                sb.Append("at every station.");
                return;
            }

            sb.Append("only at ");
            for (var i = index + 1; i < intermediateStops; i++)
            {
                if (i != index + 1 && i == intermediateStops - 1)
                    sb.Append(" and ");
                else if (i != index + 1)
                    sb.Append(", ");
                sb.Append(route.IntermediateStops[i].Station.name);
            }

            sb.Append('.');
        }

        public static string Departs(AnnouncementContext<IDeparture> context, int index)
        {
            var sb = new StringBuilder()
                .Append(context.Type)
                .Append(" ship departs for ")
                .Append(context.Destination)
                .Append(" from dock ")
                .Append(context.Dock)
                .Append(" at ")
                .Append(context.Stop.Departure)
                .Append('.');
            AppendIntermediateStops(context.Route, index, sb);
            return sb.ToString();
        }

    }

}

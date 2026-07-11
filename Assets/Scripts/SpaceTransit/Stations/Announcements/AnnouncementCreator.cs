using System.Text;
using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Ships;
using SpaceTransit.Stations.Announcements.Katilects;

namespace SpaceTransit.Stations.Announcements
{

    public static class AnnouncementCreator
    {

        public const string PleaseBoard = "Please board the ship.";

        public static bool AnyShip(this IStop stop)
        {
            foreach (var assembly in ShipAssembly.Instances)
                if (!assembly.Parent.IsRestarting && assembly.Parent.TryGetVaulter(out var vaulter) && vaulter.Stop == stop)
                    return true;
            return false;
        }

        public static IKatilect Or(this IKatilect katilect, IKatilect fallback) => katilect ?? fallback ?? IKatilect.Default;

        public static StringBuilder AppendIntermediateStops(this StringBuilder sb, RouteDescriptor route, int index)
        {
            var intermediateStops = route.IntermediateStops.Length;
            if (index >= intermediateStops - 1)
                return sb;
            sb.Append(" The ship stops ");
            if (route.EveryStation)
                return sb.Append("at every station.").AppendConditionalStops(route, index);
            sb.Append("only at ");
            for (var i = index + 1; i < intermediateStops; i++)
            {
                if (i != index + 1 && i == intermediateStops - 1)
                    sb.Append(" and ");
                else if (i != index + 1)
                    sb.Append(", ");
                sb.Append(route.IntermediateStops[i].Station.name);
            }

            return sb.Append('.').AppendConditionalStops(route, index);
        }

        private static StringBuilder AppendConditionalStops(this StringBuilder sb, RouteDescriptor route, int index)
        {
            var intermediateStops = route.IntermediateStops.Length;
            var secondToLastComma = -1;
            var count = 0;
            for (var i = index + 1; i < intermediateStops; i++)
            {
                if (route.IntermediateStops[i] is not {Conditional: true} stop)
                    continue;
                count++;
                if (secondToLastComma == -1)
                    sb.Append(" Your attention, please. ");
                secondToLastComma = sb.Length - 2;
                sb.Append(stop.Station.name);
                sb.Append(", ");
            }

            if (count == 0)
                return sb;
            if (secondToLastComma != -1 && count != 1)
                sb.Remove(secondToLastComma, 1).Insert(secondToLastComma, " and");
            return sb.Remove(sb.Length - 2, 1)
                .Append(count == 1 ? "is a conditional stop" : "are conditional stops")
                .Append(". The ship will only stop if there are passengers waiting to board or disembark.");
        }

        public static string ArrivingAndDeparts(AnnouncementContext<IArrival> context, int index) => new StringBuilder()
            .Append(context.Type)
            .Append(" ship is arriving from ")
            .Append(context.Origin)
            .Append(" at dock ")
            .Append(context.Dock)
            .Append(" and departs for ")
            .Append(context.Destination)
            .Append('.')
            .AppendIntermediateStops(context.Route, index)
            .ToString();

        public static string Departs(AnnouncementContext<IDeparture> context, int index) => new StringBuilder()
            .Append(context.Type)
            .Append(" ship departs for ")
            .Append(context.Destination)
            .Append(" from dock ")
            .Append(context.Dock)
            .Append(" at ")
            .Append(context.Stop.Departure)
            .Append('.')
            .AppendIntermediateStops(context.Route, index)
            .ToString();

    }

}

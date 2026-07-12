using System;
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
            if (route.Announcement is {Rules: {Length: not 0} rules})
                return sb.AppendIntermediateStopRules(route, rules, index)
                    .AppendConditionalStops(route, index);
            return sb.Append(" The ship stops ")
                .AppendIntermediateStops(route, route.EveryStation, index, intermediateStops)
                .AppendConditionalStops(route, index);
        }

        private static StringBuilder AppendIntermediateStops(this StringBuilder sb, RouteDescriptor route, bool everyStation, int start, int end)
        {
            if (everyStation)
                return sb.Append("at every station.");
            sb.Append("only at ");
            for (var i = start + 1; i < end; i++)
            {
                if (i != start + 1 && i == end - 1)
                    sb.Append(" and ");
                else if (i != start + 1)
                    sb.Append(", ");
                sb.Append(route.IntermediateStops[i].Station.name);
            }

            return sb.Append('.');
        }

        public static StringBuilder AppendIntermediateStopRules(this StringBuilder sb, RouteDescriptor route, ReadOnlySpan<StopSubsetRule> rules, int index)
        {
            var first = true;
            var stops = route.IntermediateStops;
            foreach (var rule in rules)
            {
                var start = route.StopIndex(rule.Start);
                var end = Math.Min(route.StopIndex(rule.End), stops.Length);
                if (end <= index)
                    continue;
                if (first)
                    sb.Append(" To ");
                else
                    sb.Append(" From ").Append(rule.Start.name).Append(" to ");
                sb.Append(rule.End.name).Append(first ? " the ship stops " : " it stops ");
                sb.AppendIntermediateStops(route, rule.EveryStation, start, end);
                first = false;
            }

            return sb;
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

        public static StringBuilder AppendVia<T>(this StringBuilder sb, AnnouncementContext<T> context, int stopIndex, string suffix = "") where T : IStop
        {
            var via = context.Via(stopIndex);
            return via.Length == 0 ? sb : sb.AppendVia(via, suffix);
        }

        public static StringBuilder AppendVia(this StringBuilder sb, ReadOnlySpan<StationId> via, string suffix)
        {
            sb.Append(" via ");
            for (var i = 0; i < via.Length; i++)
            {
                if (i != 0)
                    sb.Append(", ");
                sb.Append(via[i].name);
            }

            return sb.Append(suffix);
        }

        public static string ArrivingAndDeparts(AnnouncementContext<IArrival> context, int index, string viaSuffix = "") => new StringBuilder()
            .Append(context.Type)
            .Append(" ship is arriving from ")
            .Append(context.Origin)
            .Append(" at dock ")
            .Append(context.Dock)
            .Append(" and departs for ")
            .Append(context.Destination)
            .AppendVia(context, index, viaSuffix)
            .Append('.')
            .AppendIntermediateStops(context.Route, index)
            .ToString();

        public static string Departs(AnnouncementContext<IDeparture> context, int index, string viaSuffix = "") => new StringBuilder()
            .Append(context.Type)
            .Append(" ship departs for ")
            .Append(context.Destination)
            .AppendVia(context, index, viaSuffix)
            .Append(" from dock ")
            .Append(context.Dock)
            .Append(" at ")
            .Append(context.Stop.Departure)
            .Append('.')
            .AppendIntermediateStops(context.Route, index)
            .ToString();

        public static ReadOnlySpan<StationId> Via<T>(this AnnouncementContext<T> context, int stopIndex = ITarget.Origin) where T : IStop
        {
            if (stopIndex == ITarget.Destination || context.Descriptor is not {Via: {Length: not 0} via, MinViaDistance: var minDistance})
                return default;
            if (stopIndex == ITarget.Origin)
                return via;
            for (var i = 0; i < via.Length; i++)
            {
                var index = context.Route.StopIndex(via[i]);
                if (index > stopIndex && Math.Abs(index - stopIndex) >= minDistance)
                    return via[i..];
            }

            return default;
        }

        public static StringBuilder AppendContextFormat(this StringBuilder sb, AnnouncementContext<IDeparture> context, string format)
            => sb.AppendFormat(format, context.Type, context.Destination, context.Dock, context.Stop.Departure);

    }

}

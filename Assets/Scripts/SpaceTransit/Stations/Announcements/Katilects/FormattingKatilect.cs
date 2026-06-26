using System.Text;
using Katie.Unity;
using SpaceTransit.Routes.Stops;
using UnityEngine;

namespace SpaceTransit.Stations.Announcements.Katilects
{

    [CreateAssetMenu(fileName = "Katilect", menuName = "SpaceTransit/Katilect")]
    public sealed class FormattingKatilect : ScriptableObject, IKatilect
    {

        private static string Build(string format, AnnouncementContext<IDeparture> context, int argument = 0, bool appendStops = false)
            => !appendStops
                ? string.Format(format, context.Type, context.Destination, context.Dock, context.Stop.Departure, argument)
                : new StringBuilder()
                    .AppendFormat(format, context.Type, context.Destination, context.Dock, context.Stop.Departure)
                    .AppendIntermediateStops(context.Route, argument)
                    .ToString();

        [Tooltip("{0} ship\n{1} destination\n{2} dock\n{3} time")]
        public string departsFor;

        [Tooltip("{0} ship\n{1} destination\n{2} dock\n{3} minutes")]
        public string departingIn;

        [Tooltip("{0} ship\n{1} destination\n{2} dock")]
        public string departingImmediately;

        [Tooltip("{0} ship\n{1} destination\n{2} dock")]
        public string departing;

        [Tooltip("{0} ship\n{1} origin\n{2} dock\n{3} destination")]
        public string arrivingAndDeparts;

        [Tooltip("{0} ship\n{1} origin\n{2} dock")]
        public string arriving;

        public PhrasePack immediatelyOverride;

        public string DepartsFor(ref AnnouncementContext<IDeparture> context, int stopIndex)
            => Build(departsFor, context, stopIndex, true);

        public string DepartingIn(ref AnnouncementContext<IDeparture> context, int minutes)
            => Build(departingIn, context);

        public string DepartingImmediately(ref AnnouncementContext<IDeparture> context)
        {
            if (immediatelyOverride)
                context.Pack = immediatelyOverride;
            return Build(departingImmediately, context);
        }

        public string Departing(ref AnnouncementContext<IDeparture> context)
            => Build(departing, context);

        public string ArrivingAndDepartsFor(ref AnnouncementContext<IArrival> context, int stopIndex)
            => new StringBuilder()
                .AppendFormat(arrivingAndDeparts, context.Type, context.Origin, context.Dock, context.Destination)
                .AppendIntermediateStops(context.Route, stopIndex)
                .ToString();

        public string Arriving(ref AnnouncementContext<IArrival> context)
            => string.Format(arriving, context.Type, context.Origin, context.Dock);

    }

}

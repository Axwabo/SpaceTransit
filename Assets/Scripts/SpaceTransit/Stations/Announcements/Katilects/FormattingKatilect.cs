using System.Text;
using Katie.Unity;
using SpaceTransit.Routes.Stops;
using UnityEngine;

namespace SpaceTransit.Stations.Announcements.Katilects
{

    [CreateAssetMenu(fileName = "Katilect", menuName = "SpaceTransit/Katilect")]
    public sealed class FormattingKatilect : ScriptableObject, IKatilect
    {

        [Header("Formats")]
        [Tooltip("{0} ship\n{1} destination\n{2} dock\n{3} time")]
        public string departsFor;

        [Tooltip("{0} ship\n{1} destination\n{2} dock\n{4} minutes")]
        public string departingIn;

        [Tooltip("{0} ship\n{1} destination\n{2} dock")]
        public string departingImmediately;

        [Tooltip("{0} ship\n{1} destination\n{2} dock")]
        public string departing;

        [Tooltip("{0} ship\n{1} origin\n{2} dock\n{3} destination")]
        public string arrivingAndDeparts;

        [Tooltip("{0} ship\n{1} origin\n{2} dock")]
        public string arriving;

        public bool announceIntermediateStops = true;

        [Header("Suffixes")]
        [Tooltip("A leading space is required")]
        public string arrivingAndDepartsSuffix;

        [Header("Pack overrides")]
        [Tooltip("Used for all \"is departing...\" announcements (but not for \"departs for...\")")]
        public PhrasePack departingOverride;

        public PhrasePack arrivingAndDepartsOverride;

        public string DepartsFor(ref AnnouncementContext<IDeparture> context, int stopIndex)
            => Build(departsFor, ref context, stopIndex, true);

        public string DepartingIn(ref AnnouncementContext<IDeparture> context, int minutes)
            => Build(departingIn, ref context, minutes);

        public string DepartingImmediately(ref AnnouncementContext<IDeparture> context)
            => Build(departingImmediately, ref context);

        public string Departing(ref AnnouncementContext<IDeparture> context)
            => Build(departing, ref context);

        public string ArrivingAndDepartsFor(ref AnnouncementContext<IArrival> context, int stopIndex)
        {
            if (arrivingAndDepartsOverride)
                context.Pack = arrivingAndDepartsOverride;
            return new StringBuilder()
                .AppendFormat(arrivingAndDeparts, context.Type, context.Origin, context.Dock, context.Destination)
                .AppendIntermediateStops(context.Route, Index(stopIndex))
                .Append(arrivingAndDepartsSuffix)
                .ToString();
        }

        public string Arriving(ref AnnouncementContext<IArrival> context)
            => string.Format(arriving, context.Type, context.Origin, context.Dock);

        private string Build(string format, ref AnnouncementContext<IDeparture> context, int argument = 0, bool stops = false)
        {
            if (stops)
                return new StringBuilder()
                    .AppendFormat(format, context.Type, context.Destination, context.Dock, context.Stop.Departure)
                    .AppendIntermediateStops(context.Route, Index(argument))
                    .ToString();
            if (string.IsNullOrWhiteSpace(format))
                return null;
            if (departingOverride)
                context.Pack = departingOverride;
            return string.Format(format, context.Type, context.Destination, context.Dock, context.Stop.Departure, argument);
        }

        private int Index(int stopIndex) => announceIntermediateStops ? stopIndex : int.MaxValue;

    }

}

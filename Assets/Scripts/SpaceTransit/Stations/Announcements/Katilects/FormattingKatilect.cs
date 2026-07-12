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

        [Tooltip("{0} ship\n{1} destination\n{2} dock\n{3} time")]
        public string departsForBeforeVia;

        [Tooltip("{0} ship\n{1} destination\n{2} dock\n{3} time")]
        public string departsForAfterVia;

        [Tooltip("{0} ship\n{1} destination\n{2} dock\n{4} minutes")]
        public string departingIn;

        [Tooltip("{0} ship\n{1} destination\n{2} dock")]
        public string departingImmediately;

        [Tooltip("{0} ship\n{1} destination\n{2} dock")]
        public string departing;

        [Tooltip("{0} ship\n{1} origin\n{2} dock\n{3} destination")]
        public string arrivingAndDeparts;

        [Tooltip("{0} ship\n{1} origin\n{2} dock\n{3} destination")]
        public string arrivingAndDepartsBeforeVia;

        [Tooltip("{0} ship\n{1} origin\n{2} dock\n{3} destination")]
        public string arrivingAndDepartsAfterVia;

        [Tooltip("{0} ship\n{1} origin\n{2} dock")]
        public string arriving;

        public bool announceIntermediateStops = true;

        [Header("Suffixes")]
        [Tooltip("A leading space is required")]
        public string arrivingAndDepartsSuffix;

        [SerializeField]
        [Tooltip("A leading space is required")]
        public string viaSuffix;

        [Header("Pack overrides")]
        [Tooltip("Used for all \"is departing...\" announcements (but not for \"departs for...\")")]
        public PhrasePack departingOverride;

        public PhrasePack arrivingAndDepartsOverride;

        public string DepartsFor(ref AnnouncementContext<IDeparture> context, int stopIndex)
            => BuildVia(context, stopIndex, departsFor, departsForBeforeVia, departsForAfterVia).ToString();

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
            return BuildVia(context, stopIndex, arrivingAndDeparts, arrivingAndDepartsBeforeVia, arrivingAndDepartsAfterVia)
                .Append(arrivingAndDepartsSuffix)
                .ToString();
        }

        public string Arriving(ref AnnouncementContext<IArrival> context)
            => string.Format(arriving, context.Type, context.Origin, context.Dock);

        private string Build(string format, ref AnnouncementContext<IDeparture> context, int argument = 0)
        {
            if (string.IsNullOrWhiteSpace(format))
                return null;
            if (departingOverride)
                context.Pack = departingOverride;
            return string.Format(format, context.Type, context.Destination, context.Dock, context.Stop.Departure, argument);
        }

        private StringBuilder BuildVia<T>(AnnouncementContext<T> context, int stopIndex, string simple, string before, string after) where T : IStop
        {
            var sb = new StringBuilder();
            var via = context.Via(stopIndex);
            if (via.Length == 0)
                sb.AppendContextFormat(context, simple);
            else
                sb.AppendContextFormat(context, before)
                    .AppendVia(via, viaSuffix)
                    .Append(' ')
                    .AppendContextFormat(context, after);
            return sb.AppendIntermediateStops(context.Route, Index(stopIndex));
        }

        private int Index(int stopIndex) => announceIntermediateStops ? stopIndex : int.MaxValue;

    }

}

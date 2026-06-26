using System;
using System.Text;
using SpaceTransit.Routes.Stops;
using UnityEngine;

namespace SpaceTransit.Stations.Announcements.Katilects
{

    [CreateAssetMenu(fileName = "Katilect", menuName = "SpaceTransit/Katilect")]
    public sealed class FormattingKatilect : ScriptableObject, IKatilect
    {

        [Tooltip("{0} ship\n{1} destination\n{2} dock\n{3} time")]
        public string departsFor;

        public string DepartsFor(ref AnnouncementContext<IDeparture> context, int stopIndex) => new StringBuilder()
            .AppendFormat(departsFor, context.Type, context.Destination, context.Dock, context.Stop.Departure)
            .AppendIntermediateStops(context.Route, stopIndex)
            .ToString();

        public string DepartingIn(ref AnnouncementContext<IDeparture> context, int minutes) => throw new NotImplementedException();

        public string DepartingImmediately(ref AnnouncementContext<IDeparture> context) => throw new NotImplementedException();

        public string Departing(ref AnnouncementContext<IDeparture> context) => throw new NotImplementedException();

        public string ArrivingAndDepartsFor(ref AnnouncementContext<IArrival> context, int stopIndex) => throw new NotImplementedException();

        public string Arriving(ref AnnouncementContext<IArrival> context) => throw new NotImplementedException();

    }

}

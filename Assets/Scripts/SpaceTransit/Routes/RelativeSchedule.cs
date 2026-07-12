using SpaceTransit.Routes.Stops;
using SpaceTransit.Stations.Announcements.Katilects;
using UnityEngine;

namespace SpaceTransit.Routes
{

    [CreateAssetMenu(fileName = "Relative Schedule", menuName = "SpaceTransit/Relative Schedule", order = 0)]
    public sealed class RelativeSchedule : ScriptableObject
    {

        public IntermediateStop[] intermediateStops;

        public Passthrough[] passthrough;

        public FormattingKatilect katilectOverride;

        public AnnouncementDescriptor announcement;

    }

}

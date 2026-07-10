using SpaceTransit.Routes;
using UnityEngine;

namespace SpaceTransit.Vaulter
{

    public static class Cache
    {

        private static JourneyDescriptorBase[] _journeys;

        private static StationId[] _stations;

        public static JourneyDescriptorBase[] Journeys => _journeys ??= Resources.LoadAll<JourneyDescriptorBase>("Routes");

        public static StationId[] Stations => _stations ??= Resources.LoadAll<StationId>("Stations");

    }

}

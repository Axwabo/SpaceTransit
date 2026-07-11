using SpaceTransit.Routes;

namespace SpaceTransit.Stations.Announcements
{

    public static class Priorities
    {

        private const int IntermediateBase = 20;
        private const int DepartingBase = 10;

        public const int NonScheduled = 100;

        public const int PassingThrough = 90;

        public const int Restarting = 50;

        public static int IntermediateDeparting(ServiceType type) => (int) type + IntermediateBase;

        public static int Departing(ServiceType type) => (int) type + DepartingBase;

        public static int ArrivingDestination(ServiceType type) => (int) type;

    }

}

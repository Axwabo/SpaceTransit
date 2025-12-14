using SpaceTransit.Cosmos;
using SpaceTransit.Routes;
using UnityEngine;

namespace SpaceTransit
{

    public sealed class World : MonoBehaviour
    {

        public const float MetersToWorld = 0.1f;
        public const float WorldToMeters = 10;

        [SerializeField]
        private SpeedLimitSign speedLimitSignPrefab;

        [SerializeField]
        private Transform stationSignPrefab;

        [SerializeField]
        private RouteDescriptor[] extraRoutes;

        public static SpeedLimitSign SpeedLimitSignPrefab { get; private set; }

        public static Transform StationSignPrefab { get; private set; }

        public static Transform Current { get; private set; }

        public static RouteDescriptor[] ExtraRoutes { get; private set; }

        private void Awake()
        {
            Current = transform;
            SpeedLimitSignPrefab = speedLimitSignPrefab;
            StationSignPrefab = stationSignPrefab;
            ExtraRoutes = extraRoutes;
        }

    }

}

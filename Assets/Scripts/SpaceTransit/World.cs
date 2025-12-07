using SpaceTransit.Cosmos;
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

        public static SpeedLimitSign SpeedLimitSignPrefab { get; private set; }

        public static Transform StationSignPrefab { get; private set; }

        public static Transform Current { get; private set; }

        private void Awake()
        {
            Current = transform;
            SpeedLimitSignPrefab = speedLimitSignPrefab;
            StationSignPrefab = stationSignPrefab;
        }

    }

}

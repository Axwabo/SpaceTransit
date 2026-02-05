using System.Collections.Generic;
using SpaceTransit.Cosmos;
using SpaceTransit.Routes;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        public static Dictionary<int, Transform> Worlds { get; } = new();

        public static SpeedLimitSign SpeedLimitSignPrefab { get; private set; }

        public static Transform StationSignPrefab { get; private set; }

        public static Transform Current { get; private set; }

        public static RouteDescriptor[] ExtraRoutes { get; private set; }

        private int _line;

        private void Awake()
        {
            if (int.TryParse(gameObject.scene.name, out var line))
                Worlds[_line = line] = transform;
            if (Current)
                return;
            Current = transform;
            SpeedLimitSignPrefab = speedLimitSignPrefab;
            StationSignPrefab = stationSignPrefab;
            ExtraRoutes = extraRoutes;
        }

        private void Start()
        {
            if (!Current)
                return;
            var t = transform;
            if (Current != t)
                t.SetParent(Current, false);
        }

        private void OnDestroy() => Worlds.Remove(_line);

        public static bool IsLoaded(int line) => Worlds.ContainsKey(line);

        public static void Load(int line)
        {
            if (line != 0 && !IsLoaded(line))
                SceneManager.LoadSceneAsync(line.ToString(), LoadSceneMode.Additive);
        }

        public static void Unload(int line)
        {
            if (line == 0 || !Worlds.TryGetValue(line, out var world))
                return;
            world.parent = null;
            SceneManager.MoveGameObjectToScene(world.gameObject, SceneManager.GetSceneByName(line.ToString()));
            SceneManager.UnloadSceneAsync(line.ToString());
        }

    }

}

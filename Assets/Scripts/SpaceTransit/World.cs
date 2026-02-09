using System;
using System.Collections.Generic;
using System.Linq;
using SpaceTransit.Cosmos;
using SpaceTransit.Movement;
using SpaceTransit.Routes;
using SpaceTransit.Ships;
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

        public static AsyncOperation LoadScene(int line)
            => line == 0 || IsLoaded(line)
                ? null
                : SceneManager.LoadSceneAsync(line.ToString(), LoadSceneMode.Additive);

        public static void Unload(params int[] lines)
        {
            foreach (var line in lines)
            {
                if (line == 0 || !Worlds.TryGetValue(line, out var world))
                    continue;
                world.parent = null;
                if (Current == world)
                    ChangeCurrentWorld(lines);
                MoveWorld(line, world);
                SceneManager.UnloadSceneAsync(line.ToString());
            }
        }

        private static void ChangeCurrentWorld(int[] unloading)
        {
            foreach (var (line, t) in Worlds)
            {
                if (unloading.Contains(line))
                    continue;
                t.parent = null;
                MoveWorld(line, t);
                Current = t;
                foreach (var assembly in ShipAssembly.Instances)
                    if (assembly.IsPlayerMounted
                        ||
                        assembly.Parent.TryGetVaulter(out var controller)
                        && controller.IsInService
                        && controller.Stop.Station.Lines.IndexOfAny(unloading) == -1)
                        assembly.Transform.parent = t;
                if (!MovementController.Current.IsMounted)
                    MovementController.Current.transform.parent = t;
                break;
            }
        }

        private static void MoveWorld(int line, Transform world) => SceneManager.MoveGameObjectToScene(world.gameObject, SceneManager.GetSceneByName(line.ToString()));

    }

}

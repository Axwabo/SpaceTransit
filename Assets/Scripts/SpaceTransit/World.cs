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

        public static Dictionary<string, Transform> Worlds { get; } = new();

        public static SpeedLimitSign SpeedLimitSignPrefab { get; private set; }

        public static Transform StationSignPrefab { get; private set; }

        public static Transform Current { get; private set; }

        public static RouteDescriptor[] ExtraRoutes { get; private set; }

        private string _line;

        private void Awake()
        {
            var line = gameObject.scene.name;
            if (!string.IsNullOrEmpty(line))
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

        private void OnDisable() => Worlds.Remove(_line);

        public static bool IsLoaded(string line) => Worlds.ContainsKey(line);

        public static AsyncOperation LoadScene(string line)
            => string.IsNullOrEmpty(line) || IsLoaded(line)
                ? null
                : SceneManager.LoadSceneAsync(line, LoadSceneMode.Additive);

        public static void Unload(params string[] lines)
        {
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line) || !Worlds.TryGetValue(line, out var world))
                    continue;
                world.parent = null;
                if (Current == world)
                    ChangeCurrentWorld(lines);
                MoveWorld(line, world);
                SceneManager.UnloadSceneAsync(line);
            }
        }

        private static void ChangeCurrentWorld(string[] unloading)
        {
            Current = null;
            foreach (var (line, t) in Worlds)
            {
                if (unloading.Contains(line))
                    continue;

                if (Current)
                {
                    t.parent = Current;
                    continue;
                }

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
            }
        }

        private static void MoveWorld(string line, Transform world) => SceneManager.MoveGameObjectToScene(world.gameObject, SceneManager.GetSceneByName(line));

    }

}

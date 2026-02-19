using System;
using System.Threading;
using System.Threading.Tasks;
using SpaceTransit.Cosmos;
using SpaceTransit.Loader;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Ships;
using SpaceTransit.Vaulter;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SpaceTransit.Routes.Sequences
{

    public static class RouteManager
    {

        private static ServiceSequence[] _sequences;

        public static ServiceSequence[] Sequences => _sequences ??= Resources.LoadAll<ServiceSequence>("Services");

        public static void Start()
        {
            var token = WorldChanger.Cts.Token;
            foreach (var sequence in Sequences)
                _ = Start(sequence, token);
        }

        private static (SpawnLocation, int) GetSpawnLocation(ServiceSequence sequence)
        {
            for (var i = 0; i < sequence.routes.Length; i++)
            {
                var route = sequence.routes[i];
                if (route.Origin.Departure > Clock.Now)
                    return route.Origin.Station.IsLoaded()
                        ? (new DockSpawn(route.Origin.DockIndex), i)
                        : (null, -1);
                // TODO: intermediate stops
            }

            return (null, -1);
        }

        private static async Awaitable Start(ServiceSequence sequence, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (GetSpawnLocation(sequence) is ({ } spawn, var index))
                        await Run(sequence, spawn, index);
                }
                catch (OperationCanceledException)
                {
                }

                await Awaitable.WaitForSecondsAsync(5, token);
            }
        }

        private static async Awaitable Run(ServiceSequence sequence, SpawnLocation spawn, int index)
        {
            var ship = Spawn(sequence, index);
            ship.initialStopIndex = spawn.StopIndex;
            if (spawn is EntrySpawn entrySpawn)
            {
                var assembly = ship.GetComponent<ShipAssembly>();
                assembly.startTube = entrySpawn.Tube;
                entrySpawn.Entry.Lock(assembly);
                if (entrySpawn.Tube.Safety is LockBasedSafety lockBasedSafety)
                    lockBasedSafety.Claim(assembly);
            }

            var token = ship.destroyCancellationToken;
            while (!token.IsCancellationRequested)
            {
                await Awaitable.WaitForSecondsAsync(5, token);
                if (ship.Stop is not Destination || ship.Parent.State != ShipState.Docked || !ship.Assembly.IsStationary() || ship.Assembly.IsManuallyDriven)
                    continue;
                await Awaitable.WaitForSecondsAsync(60, token);
                if (++index < sequence.routes.Length)
                {
                    ship.BeginRoute(sequence.routes[index]);
                    continue;
                }

                index = 0;
                await WaitForTomorrow(sequence.routes[0].Origin.Departure.Value - TimeSpan.FromHours(1), token);
                ship.BeginRoute(sequence.routes[0]);
            }
        }

        private static VaulterController Spawn(ServiceSequence sequence, int index)
        {
            var ship = Object.Instantiate(sequence.prefab, World.Current);
            ship.initialRoute = sequence.routes[index];
            return ship;
        }

        private static async Awaitable WaitForTomorrow(TimeSpan time, CancellationToken token)
        {
            var day = Clock.Date.Day;
            while (day == Clock.Date.Day || time > Clock.Now)
                await Awaitable.WaitForSecondsAsync(5, token);
        }

    }

}

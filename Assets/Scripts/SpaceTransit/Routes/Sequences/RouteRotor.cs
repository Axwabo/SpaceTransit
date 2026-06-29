using System;
using System.Threading;
using SpaceTransit.Cosmos;
using SpaceTransit.Loader;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Ships;
using SpaceTransit.Vaulter;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SpaceTransit.Routes.Sequences
{

    public static class RouteRotor
    {

        public const int UpdateInterval = 5;

        public static async Awaitable Run(ServiceSequence sequence, SpawnLocation spawn, int index)
        {
            var ship = await Spawn(sequence, spawn, index);
            var token = ship.destroyCancellationToken;
            if (spawn is TubeSpawn {StopIndex: not -1})
            {
                await Awaitable.NextFrameAsync(token);
                ship.Parent.MarkReady();
            }

            while (!token.IsCancellationRequested)
            {
                await WaitOrUnloadAsync(ship, token);
                if (index == -1 || index >= sequence.routes.Length)
                {
                    index = 0;
                    await TomorrowAsync(ship, sequence.routes[0].Origin.Departure.Value - TimeSpan.FromHours(1), token);
                    ship.BeginRoute(sequence.routes[0]);
                    continue;
                }

                if (!CompletedRoute(ship))
                    continue;
                for (var i = 0; i < 60; i += UpdateInterval)
                {
                    // TODO: make it random
                    if (i == 20)
                        await ship.Parent.RestartAsync(token);
                    await WaitOrUnloadAsync(ship, token);
                }

                ship.BeginRoute(sequence.routes[++index]);
            }
        }

        private static bool CompletedRoute(VaulterController ship)
            => ship.Stop is Destination {Station: var station}
               && ship.Parent.State == ShipState.Docked
               && ship.Assembly.IsStationary()
               && !ship.Assembly.IsManuallyDriven
               && ship.Assembly.FrontModule.Thruster.Tube is Dock dock
               && dock.Station.ID == station;

        private static async Awaitable<VaulterController> Spawn(ServiceSequence sequence, SpawnLocation spawn, int index)
        {
            // why tf is the result an array
            var ship = (await Object.InstantiateAsync(sequence.prefab, new InstantiateParameters
            {
                parent = World.Current,
                worldSpace = false
            }))[0];
            if (index != -1 && index < sequence.routes.Length)
                ship.initialRoute = sequence.routes[index];
            ship.initialStopIndex = spawn.StopIndex;
            if (spawn is not TubeSpawn tubeSpawn)
                return ship;
            var assembly = ship.GetComponent<ShipAssembly>();
            assembly.startTube = tubeSpawn.Tube;
            if (assembly.startTube.Safety is IOpposingTrafficSafety safety && ship.initialRoute)
                safety.Clearance.Claim(assembly, ship.initialRoute.Reverse);
            if (spawn is not EntrySpawn entrySpawn)
                return ship;
            await Awaitable.NextFrameAsync();
            entrySpawn.Entry.Lock(assembly);
            return ship;
        }

        private static async Awaitable TomorrowAsync(VaulterController ship, TimeSpan time, CancellationToken token)
        {
            var day = Clock.Date.Day;
            while (day == Clock.Date.Day || time > Clock.Now)
                await WaitOrUnloadAsync(ship, token);
        }

        private static async Awaitable WaitOrUnloadAsync(VaulterController ship, CancellationToken token)
        {
            await Awaitable.WaitForSecondsAsync(UpdateInterval, token);
            if (LoadingProgress.Current != null
                || ship.Assembly.IsPlayerMounted
                || ship.IsInService && ship.Stop.Station.IsLoaded()
                || ship.Assembly.FrontModule.Thruster.Tube is Dock dock && dock.Station.ID.IsLoaded())
                return;
            Object.Destroy(ship.gameObject);
            throw new OperationCanceledException("Ship was unloaded");
        }

    }

}

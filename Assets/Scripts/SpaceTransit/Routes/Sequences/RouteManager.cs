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

    public static class RouteManager
    {

        private static readonly (SpawnLocation, int) None = (null, -1);

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
            for (var routeIndex = 0; routeIndex < sequence.routes.Length; routeIndex++)
            {
                var route = sequence.routes[routeIndex];
                if (route.Origin.Departure > Clock.Now)
                    return route.Origin.Station.IsLoaded()
                        ? (new DockSpawn(route.Origin.DockIndex), routeIndex)
                        : None;
                for (var stopIndex = 0; stopIndex < route.IntermediateStops.Length; stopIndex++)
                {
                    var stop = route.IntermediateStops[stopIndex];
                    if (!Station.TryGetLoadedStation(stop.Station, out var station))
                        return None;
                    if (stop.Departure < Clock.Now + TimeSpan.FromMinutes(stop.MinStayMinutes + 1))
                        continue;
                    if (stop.Arrival <= Clock.Now)
                        return (new DockSpawn(stop.DockIndex, stopIndex), routeIndex);
                    if (stop.Arrival >= Clock.Now + TimeSpan.FromMinutes(1))
                        return Enter(station, stop, route, stopIndex, routeIndex);
                }
            }

            return None;
        }

        private static (SpawnLocation, int) Enter(Station station, IntermediateStop stop, RouteDescriptor route, int stopIndex, int routeIndex)
        {
            var dock = station.Docks[stop.DockIndex];
            var entries = route.Reverse ? dock.FrontEntries : dock.BackEntries;
            if (entries.Length == 0)
            {
                var next = dock.Next(!route.Reverse);
                if (!next)
                    return (new DockSpawn(stop.DockIndex, stopIndex), routeIndex);
                var second = dock.Next(!route.Reverse);
                return (new TubeSpawn(second ? second : next, stopIndex), routeIndex);
            }

            var finalEntry = entries[0];
            foreach (var entry in entries)
            {
                if (entry.Connected != stop.ArriveFrom)
                    continue;
                finalEntry = entry;
                break;
            }

            if (!finalEntry.IsFree || !finalEntry.Ensurer)
                return None;
            var tube = finalEntry.Ensurer.Tube.Next;
            var finalTube = route.Reverse ? tube.Next : tube;
            return finalTube
                ? (new EntrySpawn(tube, finalEntry, stopIndex), routeIndex)
                : (new DockSpawn(stop.DockIndex, stopIndex), routeIndex);
        }

        private static async Awaitable Start(ServiceSequence sequence, CancellationToken token)
        {
            await Awaitable.NextFrameAsync(token);
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
            var ship = Spawn(sequence, spawn, index);
            var token = ship.destroyCancellationToken;
            if (spawn is TubeSpawn)
            {
                await Awaitable.NextFrameAsync(token);
                ship.Parent.MarkReady();
            }

            while (!token.IsCancellationRequested)
            {
                await Awaitable.WaitForSecondsAsync(5, token);
                if (!CompletedRoute(ship))
                    continue;
                await Awaitable.WaitForSecondsAsync(60, token);
                if (++index < sequence.routes.Length)
                {
                    ship.BeginRoute(sequence.routes[index]);
                    continue;
                }

                index = 0;
                await TomorrowAsync(sequence.routes[0].Origin.Departure.Value - TimeSpan.FromHours(1), token);
                ship.BeginRoute(sequence.routes[0]);
            }
        }

        private static bool CompletedRoute(VaulterController ship) => ship.Stop is Destination && ship.Parent.State == ShipState.Docked && ship.Assembly.IsStationary() && !ship.Assembly.IsManuallyDriven;

        private static VaulterController Spawn(ServiceSequence sequence, SpawnLocation spawn, int index)
        {
            var ship = Object.Instantiate(sequence.prefab, World.Current);
            if (index < sequence.routes.Length)
                ship.initialRoute = sequence.routes[index];
            ship.initialStopIndex = spawn.StopIndex;
            if (spawn is not TubeSpawn tubeSpawn)
                return ship;
            var assembly = ship.GetComponent<ShipAssembly>();
            assembly.startTube = tubeSpawn.Tube;
            if (spawn is not EntrySpawn entrySpawn)
                return ship;
            entrySpawn.Entry.Lock(assembly);
            if (entrySpawn.Tube.Safety is LockBasedSafety lockBasedSafety)
                lockBasedSafety.Claim(assembly);
            return ship;
        }

        private static async Awaitable TomorrowAsync(TimeSpan time, CancellationToken token)
        {
            var day = Clock.Date.Day;
            while (day == Clock.Date.Day || time > Clock.Now)
                await Awaitable.WaitForSecondsAsync(5, token);
        }

    }

}

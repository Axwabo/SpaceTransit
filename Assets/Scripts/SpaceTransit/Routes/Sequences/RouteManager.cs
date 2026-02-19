using System;
using System.Threading;
using SpaceTransit.Loader;
using SpaceTransit.Routes.Stops;
using UnityEngine;

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
                        ? (SpawnLocation.Origin, routeIndex)
                        : None;
                for (var stopIndex = 0; stopIndex < route.IntermediateStops.Length; stopIndex++)
                {
                    var stop = route.IntermediateStops[stopIndex];
                    if (stop.Departure >= Clock.Now + TimeSpan.FromMinutes(stop.MinStayMinutes + 1))
                        return !Station.TryGetLoadedStation(stop.Station, out var station)
                            ? None
                            : stop.Arrival <= Clock.Now
                                ? (new SpawnLocation(stopIndex), routeIndex)
                                : stop.Arrival <= Clock.Now + TimeSpan.FromMinutes(1)
                                    ? Enter(station, stop, route, stopIndex, routeIndex)
                                    : None;
                }
            }

            var finalDestination = sequence.routes[^1].Destination;
            return Station.TryGetLoadedStation(finalDestination.Station, out var finalStation)
                ? (new TubeSpawn(finalStation.Docks[finalDestination.DockIndex]), -1)
                : None;
        }

        private static (SpawnLocation, int) Enter(Station station, IntermediateStop stop, RouteDescriptor route, int stopIndex, int routeIndex)
        {
            var dock = station.Docks[stop.DockIndex];
            var entries = route.Reverse ? dock.FrontEntries : dock.BackEntries;
            if (entries.Length == 0)
            {
                var next = dock.Next(!route.Reverse);
                if (!next)
                    return (new SpawnLocation(stopIndex), routeIndex);
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
            var tube = finalEntry.Ensurer.Tube;
            var finalTube = route.Reverse ? tube.Next : tube;
            return finalTube
                ? (new EntrySpawn(tube, finalEntry, stopIndex), routeIndex)
                : (new SpawnLocation(stopIndex), routeIndex);
        }

        private static async Awaitable Start(ServiceSequence sequence, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (GetSpawnLocation(sequence) is ({ } spawn, var index))
                        await RouteRotor.Run(sequence, spawn, index);
                }
                catch (OperationCanceledException)
                {
                }

                await Awaitable.WaitForSecondsAsync(RouteRotor.UpdateInterval, token);
            }
        }

    }

}

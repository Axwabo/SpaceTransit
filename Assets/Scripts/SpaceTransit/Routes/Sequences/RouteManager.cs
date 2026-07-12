using System;
using System.Linq;
using System.Threading;
using SpaceTransit.Loader;
using SpaceTransit.Routes.Stops;
using UnityEngine;

namespace SpaceTransit.Routes.Sequences
{

    public static class RouteManager
    {

        private static readonly (SpawnLocation, int) None = (null, ITarget.Origin);

        private static ServiceSequence[] _sequences;

        public static ServiceSequence[] Sequences => _sequences ??= Resources.LoadAll<ServiceSequence>("Services");

        public static void Start() => _ = StartAll();

        private static async Awaitable StartAll()
        {
            var token = WorldChanger.Cts.Token;
            foreach (var sequence in Sequences.OrderBy(GetNextSortableDeparture))
                if (Start(sequence, token))
                    await Awaitable.NextFrameAsync(token);
        }

        private static TimeSpan GetNextSortableDeparture(ServiceSequence sequence)
        {
            foreach (var descriptor in sequence.routes)
            {
                if (descriptor is not RouteDescriptor route)
                    continue;
                var time = route.Origin.Departure.Value;
                if (time > Clock.Now)
                    return time;
            }

            return TimeSpan.MaxValue;
        }

        private static (SpawnLocation, int) GetSpawnLocation(ServiceSequence sequence)
        {
            for (var routeIndex = 0; routeIndex < sequence.routes.Length; routeIndex++)
            {
                var route = sequence.routes[routeIndex];
                if (route.Beginning.Departure > Clock.Now)
                    return Station.TryGetLoadedStation(route.Beginning.Station, out var departureStation) && departureStation.Docks[route.Beginning.DockIndex].IsFree
                        ? (SpawnLocation.Origin, routeIndex)
                        : None;
                for (var stopIndex = 0; stopIndex < route.IntermediateStops.Length; stopIndex++)
                {
                    var stop = route.IntermediateStops[stopIndex];
                    if (stop.Departure >= Clock.Now + TimeSpan.FromMinutes(stop.MinStayMinutes + 1))
                        return !Station.TryGetLoadedStation(stop.Station, out var station)
                            ? None
                            : stop.Arrival <= Clock.Now
                                ? SpawnAt(station, stop, stopIndex, routeIndex)
                                : stop.Arrival <= Clock.Now + TimeSpan.FromMinutes(1)
                                    ? Enter(station, stop, route, stopIndex, routeIndex)
                                    : None;
                }

                if (EnterDestination(route, out var location))
                    return (location, routeIndex);
            }

            return EnterDestination(sequence.routes[^1], out var finalLocation)
                ? (finalLocation, sequence.routes.Length - 1)
                : None;
        }

        private static bool EnterDestination(JourneyDescriptorBase journey, out SpawnLocation location)
        {
            if (journey.End is not Destination destination
                || destination.Arrival > Clock.Now + TimeSpan.FromMinutes(1)
                || !Station.TryGetLoadedStation(destination.Station, out var station))
            {
                location = null;
                return false;
            }

            location = Enter(station, destination, journey, ITarget.Destination, ITarget.Destination).Item1;
            return location is not null;
        }

        private static (SpawnLocation, int) SpawnAt(Station station, IntermediateStop stop, int stopIndex, int routeIndex)
            => !station.Docks[stop.DockIndex].IsFree
                ? None
                : (new SpawnLocation(stopIndex), routeIndex);

        private static (SpawnLocation, int) Enter(Station station, IArrival stop, JourneyDescriptorBase descriptor, int stopIndex, int routeIndex)
        {
            var dock = station.Docks[stop.DockIndex];
            if (!dock.IsFree)
                return None;
            var entries = descriptor.Reverse ? dock.FrontEntries : dock.BackEntries;
            if (entries.Length == 0)
            {
                var next = dock.Next(!descriptor.Reverse);
                if (!next)
                    return (new SpawnLocation(stopIndex), routeIndex);
                var second = dock.Next(!descriptor.Reverse);
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
            var finalTube = descriptor.Reverse ? tube.Next : tube;
            return finalTube
                ? (new EntrySpawn(tube, finalEntry, stopIndex), routeIndex)
                : (new SpawnLocation(stopIndex), routeIndex);
        }

        private static bool Start(ServiceSequence sequence, CancellationToken token)
        {
            if (GetSpawnLocation(sequence) is not ({ } spawn, var index))
            {
                _ = StartAsync(sequence, token);
                return false;
            }

            _ = StartAsync(sequence, spawn, index, token);
            return true;
        }

        private static async Awaitable StartAsync(ServiceSequence sequence, SpawnLocation initialSpawn, int initialIndex, CancellationToken token)
        {
            await RouteRotor.Run(sequence, initialSpawn, initialIndex);
            await StartAsync(sequence, token);
        }

        private static async Awaitable StartAsync(ServiceSequence sequence, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await Awaitable.WaitForSecondsAsync(RouteRotor.UpdateInterval, token);
                try
                {
                    if (GetSpawnLocation(sequence) is ({ } spawn, var index))
                        await RouteRotor.Run(sequence, spawn, index);
                }
                catch (OperationCanceledException)
                {
                }
            }
        }

    }

}

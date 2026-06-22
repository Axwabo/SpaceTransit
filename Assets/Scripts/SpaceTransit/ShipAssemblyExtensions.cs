using System;
using SpaceTransit.Cosmos;
using SpaceTransit.Routes;
using SpaceTransit.Ships;
using SpaceTransit.Ships.Driving.Screens;
using SpaceTransit.Tubes;
using UnityEngine;

namespace SpaceTransit
{

    public static class ShipAssemblyExtensions
    {

        public static Vector3 ClosestPoint(this ShipAssembly assembly, Vector3 position)
        {
            var closest = position;
            var closestDistance = float.MaxValue;
            foreach (var module in assembly.Modules)
            {
                var point = module.AudioBounds.Closest(position);
                var distance = Vector3.Distance(position, point);
                if (distance < closestDistance)
                    (closest, closestDistance) = (point, distance);
            }

            return closest;
        }

        public static bool IsStationary(this ShipAssembly assembly) => assembly.CurrentSpeed.Raw == 0;

        public static TubeBase NextTube(this ShipAssembly assembly) => assembly.FrontModule.Thruster.Tube.Next(assembly.Reverse);

        public static bool Lock(this ShipAssembly assembly, PickerBase picker) => picker switch
        {
            EntryPicker entryPicker => Lock(assembly, picker, entryPicker.Entry),
            ExitPicker exitPicker => Lock(assembly, picker, exitPicker.Exit),
            _ => throw new InvalidOperationException()
        };

        private static bool Lock(ShipAssembly assembly, PickerBase picker, EntryOrExit exit)
        {
            var locked = exit.Lock(assembly);
            picker.Success = locked;
            picker.Failure = !locked;
            return locked;
        }

        public static bool ShouldAnnounceNonScheduled(this ShipAssembly assembly, StationId stationId)
            => !assembly.Parent.TryGetVaulter(out var vaulter)
               || !vaulter.IsInService
               || vaulter.Stop.Station != stationId;

    }

}

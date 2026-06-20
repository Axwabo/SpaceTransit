using System;
using SpaceTransit.Cosmos;
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

        public static void Lock(this ShipAssembly assembly, PickerBase picker)
        {
            switch (picker)
            {
                case EntryPicker entryPicker:
                    Lock(assembly, picker, entryPicker.Entry);
                    break;
                case ExitPicker exitPicker:
                    Lock(assembly, picker, exitPicker.Exit);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        private static void Lock(ShipAssembly assembly, PickerBase picker, EntryOrExit exit)
        {
            var locked = exit.Lock(assembly);
            picker.Success = locked;
            picker.Failure = !locked;
        }

    }

}

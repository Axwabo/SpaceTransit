using SpaceTransit.Ships;
using UnityEngine;

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

}

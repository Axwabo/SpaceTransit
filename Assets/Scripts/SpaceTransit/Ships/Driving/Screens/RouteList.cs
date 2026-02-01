using System;
using System.Collections.Generic;
using SpaceTransit.Routes;
using SpaceTransit.Vaulter;

namespace SpaceTransit.Ships.Driving.Screens
{

    public sealed class RouteList : ListBase<RouteDescriptor, PickerBase>
    {

        private static readonly HashSet<string> AvailableExits = new();

        protected override List<RouteDescriptor> Source { get; } = new();

        private void OnEnable()
        {
            if (!Parent)
                return;
            Source.Clear();
            Source.Add(null);
            Clear();
            if (Assembly.FrontModule.Thruster.Tube is not Dock dock)
                return;
            CacheExits(dock.Station);
            Append(Cache.Routes, dock);
            Append(World.ExtraRoutes, dock);
            SetUp();
        }

        private void Append(ReadOnlySpan<RouteDescriptor> routes, Dock dock)
        {
            foreach (var route in routes)
                if (route.Origin.Station == dock.Station.ID && (AvailableExits.Count == 0 || AvailableExits.Contains(route.Origin.ExitTowards.name)))
                    Source.Add(route);
        }

        private static void CacheExits(Station station)
        {
            AvailableExits.Clear();
            foreach (var dock in station.Docks)
            {
                foreach (var exit in dock.FrontExits)
                    AvailableExits.Add(exit.Connected.name);
                foreach (var exit in dock.BackExits)
                    AvailableExits.Add(exit.Connected.name);
            }
        }

        protected override string GetContent(RouteDescriptor item)
            => item == null ? "Exit Service" : RouteDisplay.Format(item);

        protected override bool Select(RouteDescriptor item, PickerBase picker)
        {
            if (!Controller.TryGetVaulter(out var vaulter))
                return false;
            if (item)
                vaulter.BeginRoute(item);
            else
                vaulter.ExitService();
            return true;
        }

    }

}

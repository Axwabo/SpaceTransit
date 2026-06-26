using System;
using System.Collections.Generic;
using SpaceTransit.Routes;
using SpaceTransit.Tubes;
using UnityEngine;
using Cache = SpaceTransit.Vaulter.Cache;

namespace SpaceTransit.Ships.Driving.Screens
{

    [RequireComponent(typeof(RouteList))]
    public sealed class RouteListManager : ScreenManagerBase<RouteList>
    {

        private static readonly HashSet<StationId> AvailableExits = new();

        protected override void OnEnable()
        {
            if (!Parent)
                return;
            base.OnEnable();
            Refresh(Assembly.FrontModule.Thruster.Tube);
        }

        public void Refresh(TubeBase tube)
        {
            Screen.Source.Clear();
            if (tube is Dock dock)
            {
                CacheExits(dock.Station);
                if (World.ExtraRoutes.Length != 0)
                    Append(World.ExtraRoutes, dock, false);
                else
                    Append(Cache.Routes, dock, true);
            }

            Screen.Source.Insert(0, RoutePicker.ExitService);
            Screen.Refresh();
        }

        private static void CacheExits(Station station)
        {
            AvailableExits.Clear();
            foreach (var dock in station.Docks)
            {
                foreach (var exit in dock.FrontExits)
                    AvailableExits.Add(exit.Connected);
                foreach (var exit in dock.BackExits)
                    AvailableExits.Add(exit.Connected);
            }
        }

        private void Append(ReadOnlySpan<RouteDescriptor> routes, Dock dock, bool sort)
        {
            foreach (var route in routes)
                if (route.Origin.Station == dock.Station.ID && (AvailableExits.Count == 0 || AvailableExits.Contains(route.Origin.ExitTowards)))
                    Screen.Source.Add(new RoutePicker(route));
            if (sort)
                Screen.Source.Sort((a, b) =>
                {
                    int.TryParse(a.Descriptor.name, out var na);
                    int.TryParse(b.Descriptor.name, out var nb);
                    return na.CompareTo(nb);
                });
        }

        public override void OnStateChanged()
        {
            if (!isActiveAndEnabled)
                return;
            if (State == ShipState.Docked)
                Refresh(Assembly.FrontModule.Thruster.Tube);
            else
                Screen.Clear();
        }

    }

}

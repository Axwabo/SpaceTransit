using System;
using System.Collections.Generic;
using SpaceTransit.Routes;
using SpaceTransit.Ships.Modules;
using UnityEngine;
using Cache = SpaceTransit.Vaulter.Cache;

namespace SpaceTransit.Ships.Driving.Screens
{

    [RequireComponent(typeof(RouteList))]
    public sealed class RouteListManager : ModuleComponentBase, ICullingListener
    {

        private static readonly HashSet<StationId> AvailableExits = new();

        private RouteList _list;

        protected override void Awake()
        {
            base.Awake();
            _list = GetComponent<RouteList>();
        }

        private void OnEnable()
        {
            if (!Parent)
                return;
            _list.enabled = true;
            _list.Source.Clear();
            _list.Source.Add(RoutePicker.ExitService);
            if (Assembly.FrontModule.Thruster.Tube is Dock dock)
            {
                CacheExits(dock.Station);
                Append(World.ExtraRoutes.Length != 0 ? World.ExtraRoutes : Cache.Routes, dock);
            }

            _list.Refresh();
        }

        private void OnDisable() => _list.enabled = false;

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

        private void Append(ReadOnlySpan<RouteDescriptor> routes, Dock dock)
        {
            foreach (var route in routes)
                if (route.Origin.Station == dock.Station.ID && (AvailableExits.Count == 0 || AvailableExits.Contains(route.Origin.ExitTowards)))
                    _list.Source.Add(new RoutePicker(route));
        }

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using SpaceTransit.Loader;
using SpaceTransit.Movement;
using SpaceTransit.Routes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cache = SpaceTransit.Vaulter.Cache;

namespace SpaceTransit.Menu
{

    public sealed class TeleportList : MonoBehaviour
    {

        private readonly List<StationId> _stations = new();

        private Label _error;

        private ListView _list;

        private void Start()
        {
            _stations.AddRange(World.IsTestWorld ? Station.LoadedStations.Select(e => e.ID) : Cache.Stations);
            _stations.Sort((a, b) => string.Compare(a.name, b.name, StringComparison.OrdinalIgnoreCase));
            var root = this.RootVisual();
            _error = root.Q<Label>("Error");
            _list = root.Q<ListView>();
            _list.makeItem = () => new Label();
            _list.bindItem = (element, i) => (element as Label ?? element.Q<Label>()).text = _stations[i].name;
            _list.itemsSource = _stations;
            _list.selectedIndicesChanged += Click;
        }

        private void OnDisable()
        {
            if (_error != null)
                _error.text = "";
        }

        private void Click(IEnumerable<int> obj)
        {
            if (_list.selectedIndex == -1)
                return;
            var id = _stations[_list.selectedIndex];
            _list.selectedIndex = -1;
            if (LoadingProgress.Current != null)
            {
                _error.text = "Please wait for loading to complete.";
                return;
            }

            if (MovementController.Current.IsMounted)
            {
                _error.text = "You must disembark before teleporting.";
                return;
            }

            if (InputSystem.actions["Move"].ReadValue<Vector2>() != Vector2.zero)
            {
                _error.text = "You mustn't be moving when teleporting.";
                return;
            }

            if (!Station.TryGetLoadedStation(id, out var station))
            {
                _ = WorldChanger.Load(id.Lines.ToArray());
                _error.text = "Now loading, please try again later.";
                return;
            }

            _error.text = "";
            if (!World.IsTestWorld)
                World.Unload(World.Worlds.Keys.Except(station.ID.Lines.ToArray()).ToArray());
            MovementController.Current.Teleport(station.Spawnpoint);
        }

    }

}

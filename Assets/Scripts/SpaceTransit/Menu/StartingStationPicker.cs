using System;
using System.Collections.Generic;
using SpaceTransit.Movement;
using SpaceTransit.Routes;
using UnityEngine;
using UnityEngine.UIElements;
using Cache = SpaceTransit.Vaulter.Cache;

namespace SpaceTransit.Menu
{

    public sealed class StartingStationPicker : MonoBehaviour
    {

        [SerializeField]
        private string defaultValue;

        private readonly List<StationId> _stations = new();

        private void Start()
        {
            _stations.Clear();
            var list = new List<string>();
            foreach (var id in Cache.Stations)
            {
                _stations.Add(id);
                list.Add(id.name);
            }

            list.Sort(StringComparer.OrdinalIgnoreCase);
            _stations.Sort((a, b) => StringComparer.OrdinalIgnoreCase.Compare(a.name, b.name));
            var index = MovementController.StartingStation ? _stations.IndexOf(MovementController.StartingStation) : -1;
            if (index == -1)
                index = list.IndexOf(defaultValue);
            var dropdown = this.RootVisual().Q<DropdownField>("Stations");
            dropdown.choices = list;
            dropdown.index = index;
            dropdown.RegisterValueChangedCallback(_ => Change(dropdown.index));
            Change(index);
        }

        private void Change(int arg0) => MovementController.StartingStation = _stations[arg0];

    }

}

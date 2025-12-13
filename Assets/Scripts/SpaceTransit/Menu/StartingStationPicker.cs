using System;
using System.Collections.Generic;
using SpaceTransit.Movement;
using SpaceTransit.Routes;
using TMPro;
using UnityEngine;
using Cache = SpaceTransit.Vaulter.Cache;

namespace SpaceTransit.Menu
{

    [RequireComponent(typeof(TMP_Dropdown))]
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
            var index = list.IndexOf(defaultValue);
            var dropdown = GetComponent<TMP_Dropdown>();
            dropdown.AddOptions(list);
            dropdown.value = Mathf.Max(0, index);
            dropdown.onValueChanged.AddListener(Change);
            Change(index);
        }

        private void Change(int arg0) => MovementController.StartingStation = _stations[arg0];

    }

}

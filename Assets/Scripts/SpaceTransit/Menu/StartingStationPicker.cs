using System.Linq;
using SpaceTransit.Movement;
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

        private void Start()
        {
            var list = Cache.Stations.Select(static e => e.name).ToList();
            var index = list.IndexOf(defaultValue);
            var dropdown = GetComponent<TMP_Dropdown>();
            dropdown.AddOptions(list);
            dropdown.value = Mathf.Max(0, index);
            dropdown.onValueChanged.AddListener(Change);
            Change(index);
        }

        private static void Change(int arg0) => MovementController.StartingStation = Cache.Stations[arg0];

    }

}

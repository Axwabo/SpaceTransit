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
            var dropdown = GetComponent<TMP_Dropdown>();
            dropdown.AddOptions(list);
            dropdown.value = Mathf.Max(0, list.IndexOf(defaultValue));
            dropdown.onValueChanged.AddListener(Change);
        }

        private static void Change(int arg0) => MovementController.StartingStation = Cache.Stations[arg0];

    }

}

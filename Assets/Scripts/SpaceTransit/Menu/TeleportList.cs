using System.Linq;
using SpaceTransit.Movement;
using SpaceTransit.Routes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceTransit.Menu
{

    public sealed class TeleportList : MonoBehaviour
    {

        [SerializeField]
        private GameObject prefab;

        private void Start()
        {
            var t = transform;
            foreach (var station in Station.LoadedStations.OrderBy(e => e.Name))
                Instantiate(prefab, t).AddComponent<Teleport>().Station = station;
        }

        // TODO: refactor
        private sealed class Teleport : MonoBehaviour
        {

            public Station Station { get; set; }

            private void Start()
            {
                GetComponent<Button>().onClick.AddListener(Click);
                GetComponentInChildren<TextMeshProUGUI>().text = Station.Name;
            }

            private void Click()
            {
                if (!MovementController.Current.Mount)
                    MovementController.Current.Teleport(Station.Spawnpoint + Vector3.up * 0.1f);
            }

        }

    }

}

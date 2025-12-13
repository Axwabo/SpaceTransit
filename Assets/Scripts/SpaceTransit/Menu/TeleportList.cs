using System.Linq;
using SpaceTransit.Movement;
using SpaceTransit.Routes;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace SpaceTransit.Menu
{

    public sealed class TeleportList : MonoBehaviour
    {

        [SerializeField]
        private GameObject prefab;

        [SerializeField]
        private TextMeshProUGUI error;

        private void Start()
        {
            var t = transform;
            foreach (var station in Station.LoadedStations.OrderBy(e => e.Name))
            {
                var teleport = Instantiate(prefab, t).AddComponent<Teleport>();
                teleport.Station = station;
                teleport.Error = error;
            }
        }

        private void OnDisable() => error.text = "";

        // TODO: refactor
        private sealed class Teleport : MonoBehaviour
        {

            public Station Station { get; set; }

            public TextMeshProUGUI Error { get; set; }

            private void Start()
            {
                GetComponent<Button>().onClick.AddListener(Click);
                GetComponentInChildren<TextMeshProUGUI>().text = Station.Name;
            }

            private void Click()
            {
                if (MovementController.Current.Mount)
                {
                    Error.text = "You must disembark before teleporting.";
                    return;
                }

                if (InputSystem.actions["Move"].ReadValue<Vector2>() != Vector2.zero)
                {
                    Error.text = "You mustn't be moving when teleporting.";
                    return;
                }

                Error.text = "";
                MovementController.Current.Teleport(Station.Spawnpoint);
            }

        }

    }

}

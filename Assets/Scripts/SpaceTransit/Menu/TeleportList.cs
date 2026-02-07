using System.Linq;
using SpaceTransit.Loader;
using SpaceTransit.Movement;
using SpaceTransit.Routes;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Cache = SpaceTransit.Vaulter.Cache;

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
            foreach (var station in Cache.Stations.OrderBy(e => e.name))
            {
                var teleport = Instantiate(prefab, t).AddComponent<Teleport>();
                teleport.Id = station;
                teleport.Error = error;
            }
        }

        private void OnDisable() => error.text = "";

        // TODO: refactor
        private sealed class Teleport : MonoBehaviour
        {

            public StationId Id { get; set; }

            public TextMeshProUGUI Error { get; set; }

            private void Start()
            {
                GetComponent<Button>().onClick.AddListener(Click);
                GetComponentInChildren<TextMeshProUGUI>().text = Id.name;
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

                if (!Station.TryGetLoadedStation(Id, out var station))
                {
                    _ = WorldChanger.Load(Id.Lines.ToArray());
                    Error.text = "Now loading, please try again later.";
                    return;
                }

                Error.text = "";
                MovementController.Current.Teleport(station.Spawnpoint);
            }

        }

    }

}

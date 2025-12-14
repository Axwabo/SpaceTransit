using System.Linq;
using SpaceTransit.Routes;
using TMPro;
using UnityEngine;

namespace SpaceTransit.Menu
{

    public sealed class CrashDisplay : MonoBehaviour
    {

        [SerializeField]
        private TextMeshProUGUI station;

        public static CrashDisplay Current { get; private set; }

        private void Awake()
        {
            Current = this;
            gameObject.SetActive(false);
        }

        public static void DisplayCrash(Vector3 position)
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
            Cursor.lockState = CursorLockMode.None;
            var closest = Station.LoadedStations.Select(e => (Vector3.Distance(e.transform.position, position), e)).OrderBy(e => e.Item1).First();
            Current.gameObject.SetActive(true);
            Current.station.text = closest.e.Name;
        }

    }

}

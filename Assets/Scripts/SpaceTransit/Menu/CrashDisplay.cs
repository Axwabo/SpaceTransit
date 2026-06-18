using System.Linq;
using SpaceTransit.Routes;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace SpaceTransit.Menu
{

    public sealed class CrashDisplay : MonoBehaviour
    {

        public static CrashDisplay Current { get; private set; }

        private VisualElement _root;

        private Label _station;

        private void Start()
        {
            Current = this;
            _root = this.RootVisual();
            _root.SetVisibility(false);
            _station = _root.Q<Label>("Station");
        }

        public static void DisplayCrash(Vector3 position)
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
            Cursor.lockState = CursorLockMode.None;
            var closest = Station.LoadedStations.Select(e => (Vector3.Distance(e.transform.position, position), e)).OrderBy(e => e.Item1).First();
            Current._root.SetVisibility(true);
            Current._station.text = closest.e.Name;
            MenuScreen.Disable();
        }

    }

}

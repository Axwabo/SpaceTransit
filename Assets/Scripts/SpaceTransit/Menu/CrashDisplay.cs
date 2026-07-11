using System.Linq;
using SpaceTransit.Routes;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace SpaceTransit.Menu
{

    public sealed class CrashDisplay : MonoBehaviour
    {

        public static CrashDisplay Current { get; private set; }

        private VisualElement _root;

        [CreateProperty]
        public bool Visible { get; private set; }

        [CreateProperty]
        public string ClosestStation { get; private set; }

        private void Start()
        {
            Current = this;
            this.RootVisual().dataSource = this;
        }

        public static void DisplayCrash(Vector3 position)
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
            Cursor.lockState = CursorLockMode.None;
            var closest = Station.LoadedStations.Select(e => (Vector3.Distance(e.transform.position, position), e)).OrderBy(e => e.Item1).First();
            Current.Visible = true;
            Current.ClosestStation = closest.e.Name;
            MenuScreen.Disable();
        }

    }

}

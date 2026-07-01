using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceTransit.Movement
{

    public static class TouchscreenMode
    {

        private const string Key = "Touchscreen";

        public static bool Enabled { get; private set; }

        public static bool Interact { get; set; }

        public static bool Jump { get; set; }

        public static Vector2 Movement { get; set; }

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            if (PlayerPrefs.HasKey(Key))
            {
                Enabled = PlayerPrefs.GetInt(Key) == 1;
                return;
            }

            foreach (var device in InputSystem.devices)
                Enabled |= device is Touchscreen;
        }

        public static void Set(bool enabled)
        {
            Enabled = enabled;
            PlayerPrefs.SetInt(Key, enabled ? 1 : 0);
        }

    }

}

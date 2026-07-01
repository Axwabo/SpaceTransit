using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceTransit.Movement
{

    public static class TouchscreenMode
    {

        public static bool Enabled { get; set; }

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            foreach (var device in InputSystem.devices)
                Enabled |= device is Touchscreen;
        }

    }

}

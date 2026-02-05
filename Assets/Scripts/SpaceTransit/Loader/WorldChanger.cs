using UnityEngine;

namespace SpaceTransit.Loader
{

    public sealed class WorldChanger : MonoBehaviour
    {

        [SerializeField]
        private int line;

        [SerializeField]
        private bool unload;

        [SerializeField]
        private bool exit;

        private void OnTriggerEnter(Collider other)
        {
            if (!exit)
                Toggle();
        }

        private void OnTriggerExit(Collider other)
        {
            if (exit)
                Toggle();
        }

        private void Toggle()
        {
            if (unload)
                World.Unload(line);
            else
                World.Load(line);
        }

    }

}

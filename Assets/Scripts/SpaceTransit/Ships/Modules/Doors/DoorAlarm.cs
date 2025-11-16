using UnityEngine;

namespace SpaceTransit.Ships.Modules.Doors
{

    public sealed class DoorAlarm : MonoBehaviour
    {

        [SerializeField]
        private DoorController controller;

        [SerializeField]
        private AudioSource source;

        [SerializeField]
        private AudioClip beep;

        [SerializeField]
        private float interval;

        private float _remaining;

        private void Update()
        {
            if ((_remaining -= Time.deltaTime) > 0 || !controller.AlarmActive)
                return;
            source.PlayOneShot(beep);
            _remaining = interval;
        }

    }

}

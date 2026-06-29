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
        private MeshRenderer meshRenderer;

        [SerializeField]
        private AudioClip beep;

        [SerializeField]
        private Material active;

        [SerializeField]
        private float interval;

        [SerializeField]
        private int lightRate = 1;

        [SerializeField]
        private int soundRate = 1;

        [SerializeField]
        private int max;

        private Material _inactive;

        private float _remaining;

        private float _flash;

        private int _count;

        private void Awake()
        {
            _inactive = meshRenderer.sharedMaterial;
            controller.Alarm = this;
        }

        private void Update()
        {
            if (_flash > 0)
            {
                if ((_flash -= Clock.Delta) > 0)
                    return;
                source.Stop();
                Deactivate();
                return;
            }

            if (!controller.AlarmActive)
            {
                Deactivate();
                return;
            }

            if ((_remaining -= Clock.Delta) > 0 || max != 0 && _count >= max)
                return;
            if (_count % lightRate == 0)
                meshRenderer.sharedMaterial = meshRenderer.sharedMaterial == _inactive ? active : _inactive;
            if (_count % soundRate == 0)
                source.PlayOneShot(beep, controller.Smart && controller.Controller.State != ShipState.WaitingForDeparture ? 0.3f : 1);
            _remaining += interval;
            _count++;
        }

        private void Deactivate()
        {
            if (_count != 0)
                meshRenderer.sharedMaterial = _inactive;
            _remaining = 0;
            _count = 0;
        }

        public void Flash()
        {
            source.PlayOneShot(beep);
            meshRenderer.sharedMaterial = active;
            _flash = 0.2f;
        }

    }

}

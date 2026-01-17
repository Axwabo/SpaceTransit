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

        [SerializeField]
        private bool smartVolume;

        private Material _inactive;

        private float _remaining;

        private int _count;

        private void Awake() => _inactive = meshRenderer.sharedMaterial;

        private void Update()
        {
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
                source.PlayOneShot(beep, smartVolume && controller.Controller.State != ShipState.WaitingForDeparture ? 0.5f : 1);
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

    }

}

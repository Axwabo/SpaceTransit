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

        private int _count;

        private void Awake() => _inactive = meshRenderer.sharedMaterial;

        private void Update()
        {
            if (!controller.AlarmActive)
            {
                Deactivate();
                return;
            }

            if ((_remaining -= Clock.Delta) > 0 || ++_count > max && max != 0)
                return;
            if (lightRate == 1 || _count % lightRate == 0)
                meshRenderer.sharedMaterial = meshRenderer.sharedMaterial == _inactive ? active : _inactive;
            if (soundRate == 1 || _count % soundRate == 0)
                source.PlayOneShot(beep);
            _remaining += interval;
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

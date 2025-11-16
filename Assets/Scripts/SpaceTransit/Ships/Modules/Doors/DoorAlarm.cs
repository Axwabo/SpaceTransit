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
        private bool oddOnly;

        private Material _inactive;

        private float _remaining;

        private int _count;

        private void Awake() => _inactive = meshRenderer.sharedMaterial;

        private void Update()
        {
            if (!controller.AlarmActive)
            {
                if (_count != 0)
                    meshRenderer.sharedMaterial = _inactive;
                _remaining = 0;
                _count = 0;
                return;
            }

            if ((_remaining -= Time.deltaTime) > 0)
                return;
            var odd = ++_count % 2 == 1;
            if (odd || !oddOnly)
                source.PlayOneShot(beep);
            meshRenderer.sharedMaterial = odd ? active : _inactive;
            _remaining = interval;
        }

    }

}

using UnityEngine;

namespace SpaceTransit.Menu
{

    public sealed class PassByRandomly : MonoBehaviour
    {

        [SerializeField]
        private AudioSource source;

        private Transform _t;

        private float _remaining;

        private float _z;

        private Vector3 _start;

        private Vector3 _end;

        private void Awake()
        {
            _t = transform;
            _z = _t.position.z;
            _t.position = Vector3.up * 1000;
        }

        private void Update()
        {
            if (source.isPlaying)
            {
                _t.position = Vector3.Lerp(_start, _end, source.time / source.clip.length);
                return;
            }

            if ((_remaining -= Time.deltaTime) > 0)
                return;
            source.Play();
            var fromLeft = Random.value < 0.5f;
            _t.position = _start = fromLeft ? Vector3.left * 300 : Vector3.right * 300;
            _end = fromLeft ? Vector3.right * 300 : Vector3.left * 300;
            _start.z = _end.z = _z;
            _remaining = Random.Range(30, 60);
        }

    }

}

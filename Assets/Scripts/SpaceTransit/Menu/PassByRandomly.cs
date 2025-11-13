using UnityEngine;

namespace SpaceTransit.Menu
{

    public sealed class PassByRandomly : MonoBehaviour
    {

        private static readonly Vector3 StartLeft = new(-300, 0, 0);
        private static readonly Vector3 StartRight = new(300, 0, 10);
        private static readonly Vector3 EndLeft = new(300, 0, 0);
        private static readonly Vector3 EndRight = new(-300, 0, 10);

        private static bool _leftInUse;
        private static bool _rightInUse;

        [SerializeField]
        private AudioSource source;

        private Transform _t;

        private float _remaining;

        private float _z;

        private bool _fromLeft;

        private Vector3 _start;

        private Vector3 _end;

        private void Awake()
        {
            _t = transform;
            _remaining = Random.Range(1, 20);
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
            {
                if (_fromLeft)
                    _leftInUse = false;
                else
                    _rightInUse = false;
                return;
            }

            if (_leftInUse && _rightInUse)
                return;
            source.Play();
            _fromLeft = !_leftInUse && (_rightInUse || Random.value < 0.5f);
            _t.position = _start = _fromLeft ? StartLeft : StartRight;
            _end = _fromLeft ? EndLeft : EndRight;
            _start.z += _z;
            _end.z += _z;
            _remaining = Random.Range(30, 60);
            _leftInUse |= _fromLeft;
            _rightInUse |= !_fromLeft;
        }

    }

}

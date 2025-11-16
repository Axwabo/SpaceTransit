using UnityEngine;

namespace SpaceTransit.Ships.Modules.Doors
{

    public sealed class DoorController : ModuleComponentBase, IDepartureBlocker
    {

        [SerializeField]
        private AnimationCurve sideways;

        [SerializeField]
        private AnimationCurve forwards;

        [SerializeField]
        private Transform left;

        [SerializeField]
        private Transform right;

        [SerializeField]
        private AudioSource source;

        [SerializeField]
        private AudioClip open;

        [SerializeField]
        private AudioClip close;

        private float _time;

        private float _duration;

        private Vector3 _leftOffset;

        private Vector3 _rightOffset;

        private DoorState _state;

        public bool CanDepart => _state == DoorState.Closed;

        protected override void Awake()
        {
            _leftOffset = left.localPosition;
            _rightOffset = right.localPosition;
            _duration = Mathf.Max(sideways.Duration(), forwards.Duration());
        }

        public void RequestOpen()
        {
            if (State != ShipState.Docked || _state is DoorState.Opening or DoorState.Open)
                return;
            _state = DoorState.Opening;
            source.clip = open;
            source.Play();
            source.time = _time;
        }

        private void Update()
        {
            switch (_state)
            {
                case DoorState.Opening:
                    Open();
                    break;
                case DoorState.Open:
                    if ((_time -= Time.deltaTime) > 0)
                        break;
                    _state = DoorState.Closing;
                    _time = _duration;
                    source.clip = close;
                    source.Play();
                    break;
                case DoorState.Closing:
                    Close();
                    break;
            }
        }

        private void Open()
        {
            Animate();
            if ((_time += Time.deltaTime) <= _duration)
                return;
            _state = DoorState.Open;
            _time = 10;
        }

        private void Close()
        {
            Animate();
            if ((_time -= Time.deltaTime) >= 0)
                return;
            _state = DoorState.Closed;
            _time = 0;
        }

        private void Animate()
        {
            var x = sideways.Evaluate(_time);
            var z = forwards.Evaluate(_time);
            left.localPosition = _leftOffset + new Vector3(z, 0, -x);
            right.localPosition = _rightOffset + new Vector3(z, 0, x);
        }

        private enum DoorState
        {

            Closed,
            Opening,
            Open,
            Closing

        }

    }

}

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
            if (State != ShipState.Docked || _state != DoorState.Closed)
                return;
            _state = DoorState.Opening;
            _time = 0;
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
                    _time = 0;
                    break;
                case DoorState.Closing:
                    Close();
                    break;
            }
        }

        private void Open()
        {
            Animate(_time);
            if ((_time += Time.deltaTime) <= _duration)
                return;
            _state = DoorState.Open;
            _time = 10;
        }

        private void Close()
        {
            Animate(_duration - _time);
            if ((_time += Time.deltaTime) <= _duration)
                return;
            _state = DoorState.Closed;
            _time = 0;
        }

        private void Animate(float time)
        {
            var x = sideways.Evaluate(time);
            var z = forwards.Evaluate(time);
            left.localPosition = _leftOffset + new Vector3(-x, 0, z);
            right.localPosition = _rightOffset + new Vector3(x, 0, z);
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

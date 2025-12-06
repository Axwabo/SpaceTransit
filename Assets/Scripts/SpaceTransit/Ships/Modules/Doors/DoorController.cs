using SpaceTransit.Routes;
using UnityEngine;

namespace SpaceTransit.Ships.Modules.Doors
{

    public sealed class DoorController : ModuleComponentBase, IDepartureBlocker
    {

        [SerializeField]
        private bool isLeftSide;

        [SerializeField]
        private bool backwards;

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

        private float _maxSideways;

        public bool CanDepart => _state == DoorState.Closed;

        private bool IsCorrectSide => !Controller.TryGetVaulter(out var controller)
                                      || controller.IsInService
                                      && Station.TryGetLoadedStation(controller.Stop.Station, out var station)
                                      && station.Docks[controller.Stop.DockIndex] is {Left: var openLeft, Right: var openRight}
                                      && (isLeftSide ? openLeft : openRight);

        public bool AlarmActive => _state == DoorState.Closing || _state == DoorState.Open && _time < 2f;

        public float Openness { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            _leftOffset = left.localPosition;
            _rightOffset = right.localPosition;
            _maxSideways = sideways.Evaluate(ushort.MaxValue);
            _duration = Mathf.Max(sideways.Duration(), forwards.Duration());
        }

        public void RequestOpen()
        {
            if (State != ShipState.Docked || _state is DoorState.Opening or DoorState.Open || !IsCorrectSide)
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
                    Animate();
                    break;
                case DoorState.Open:
                    if ((_time -= Clock.Delta) > 0)
                        break;
                    _state = DoorState.Closing;
                    _time = _duration;
                    source.clip = close;
                    source.Play();
                    break;
                case DoorState.Closing:
                    Close();
                    Animate();
                    break;
            }
        }

        private void Open()
        {
            if ((_time += Clock.Delta) <= _duration)
                return;
            _state = DoorState.Open;
            _time = 10;
        }

        private void Close()
        {
            if ((_time -= Clock.Delta) >= 0)
                return;
            _state = DoorState.Closed;
            _time = 0;
        }

        private void Animate()
        {
            var z = sideways.Evaluate(_time);
            var x = forwards.Evaluate(_time);
            if (isLeftSide)
                x = -x;
            if (backwards)
                z = -z;
            left.localPosition = _leftOffset + new Vector3(x, 0, -z);
            right.localPosition = _rightOffset + new Vector3(x, 0, z);
            Openness = z / _maxSideways;
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

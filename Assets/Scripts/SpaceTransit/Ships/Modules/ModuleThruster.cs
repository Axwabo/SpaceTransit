using SpaceTransit.Movement;
using SpaceTransit.Tubes;

namespace SpaceTransit.Ships.Modules
{

    public sealed class ModuleThruster : ModuleComponentBase
    {

        public TubeBase Tube { get; private set; }

        private float _distance;

        private bool _updated;

        protected override void OnInitialized()
        {
            _distance = Transform.localPosition.z;
            Tube = Assembly.startTube;
            Tube.Safety.OnEntered(Parent);
            UpdateLocation();
        }

        private void Update()
        {
            _updated = false;
            if (Assembly.CurrentSpeed.Raw != 0)
                UpdateLocation();
        }

        private void FixedUpdate()
        {
            if (Assembly.CurrentSpeed.Raw == 0)
                return;
            UpdateDistance();
            if (_updated)
                return;
            UpdateLocation();
            _updated = true;
        }

        private void OnDestroy() => Tube.Safety.OnExited(Parent);

        private void UpdateLocation()
        {
            var previousPosition = Transform.localPosition;
            var (position, rotation) = Tube.Sample(_distance);
            Transform.SetLocalPositionAndRotation(position, rotation);
            if (previousPosition != position && Parent.Mount.Transform == MovementController.Current.Mount)
                World.Current.position -= Transform.TransformVector(position - previousPosition);
        }

        private void UpdateDistance()
        {
            var target = _distance + Assembly.CurrentSpeed * Clock.Delta;
            if (target > Tube.Length)
            {
                if (!Tube.HasNext)
                    return;
                Tube.Safety.OnExited(Parent);
                _distance = target - Tube.Length;
                Tube = Tube.Next;
                Tube.Safety.OnEntered(Parent);
            }
            else if (target < 0)
            {
                if (!Tube.HasPrevious)
                    return;
                Tube.Safety.OnExited(Parent);
                Tube = Tube.Previous;
                _distance = target + Tube.Length;
                Tube.Safety.OnEntered(Parent);
            }
            else
                _distance = target;
        }

    }

}

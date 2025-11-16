using System;
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
                _distance = target - Tube.Length;
                Tube = Tube.Next;
            }
            else if (target < 0)
            {
                if (!Tube.HasPrevious)
                    return;
                Tube = Tube.Previous;
                _distance = target + Tube.Length;
            }
            else
                _distance = target;
        }

    }

}

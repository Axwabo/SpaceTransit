using SpaceTransit.Movement;
using SpaceTransit.Tubes;
using UnityEngine;

namespace SpaceTransit.Ships.Modules
{

    public sealed class ModuleThruster : ModuleComponentBase
    {

        private TubeBase _tube;

        private float _distance;

        protected override void OnInitialized()
        {
            _distance = Transform.localPosition.z;
            _tube = Assembly.startTube;
            UpdateLocation();
        }

        private void FixedUpdate()
        {
            if (Assembly.CurrentSpeed.Raw == 0)
                return;
            UpdateDistance();
            UpdateLocation();
        }

        private void UpdateLocation()
        {
            var previousPosition = Transform.localPosition;
            var (position, rotation) = _tube.Sample(_distance);
            var currentPosition = position * World.WorldToMeters;
            Transform.SetLocalPositionAndRotation(currentPosition, rotation);
            if (previousPosition != currentPosition && Parent.Mount.Transform == MovementController.Current.Mount)
                World.Current.position -= Transform.TransformVector(currentPosition - previousPosition);
        }

        private void UpdateDistance()
        {
            var target = _distance + Assembly.CurrentSpeed * Time.fixedDeltaTime;
            if (target > _tube.Length)
            {
                if (!_tube.HasNext)
                    return;
                _distance = target - _tube.Length;
                _tube = _tube.Next;
            }
            else if (target < 0)
            {
                if (!_tube.HasPrevious)
                    return;
                _tube = _tube.Previous;
                _distance = target + _tube.Length;
            }
            else
                _distance = target;
        }

    }

}

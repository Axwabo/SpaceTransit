using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Ships.Driving
{

    public sealed class SpeedHandle : ModuleComponentBase
    {

        [SerializeField]
        private float start;

        [SerializeField]
        private float end;

        private float _previous;

        private void Update()
        {
            var target = Assembly.TargetSpeed.Raw;
            if (Mathf.Approximately(_previous, target))
                return;
            _previous = target;
            Transform.localRotation = Quaternion.Euler(Mathf.LerpUnclamped(start, end, target / Assembly.MaxSpeed), 0, 0);
        }

    }

}

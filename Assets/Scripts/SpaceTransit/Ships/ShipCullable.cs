using SpaceTransit.Movement;
using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Ships
{

    public sealed class ShipCullable : ShipComponentBase
    {

        [SerializeField]
        private GameObject[] gameObjects;

        [SerializeField]
        private Renderer[] renderers;

        [SerializeField]
        private bool cullModules = true;

        [SerializeField]
        private float afterShowCooldown = 1;

        [SerializeField]
        private float range = 50;

        [SerializeField]
        private bool dynamicRange;

        [SerializeField]
        private float minRangeSpeed;

        [SerializeField]
        private float maxRange = 100;

        private bool _previouslyShown = true;

        private float _cooldown;

        protected override void OnInitialized() => _cooldown = Random.value;

        private void Update()
        {
            if ((_cooldown -= Clock.Delta) > 1)
                return;
            var currentRange = dynamicRange
                ? Mathf.Lerp(range, maxRange, Mathf.InverseLerp(minRangeSpeed, Assembly.MaxSpeed, Assembly.CurrentSpeed.Raw))
                : range;
            var show = Vector3.Distance(Assembly.FrontModule.Transform.position, MovementController.Current.LastPosition) <= currentRange;
            _cooldown = 1;
            if (show == _previouslyShown || show && (_cooldown = afterShowCooldown) > 1)
                return;
            _previouslyShown = show;
            UpdateState(show);
        }

        private void UpdateState(bool show)
        {
            foreach (var o in gameObjects)
                o.SetActive(show);
            foreach (var r in renderers)
                r.enabled = show;
            if (!cullModules)
                return;
            foreach (var module in Assembly.Modules)
            foreach (var o in module.CullableObjects)
                o.SetActive(show);
        }

    }

}

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
            _cooldown++;
            var currentRange = dynamicRange
                ? Mathf.Lerp(range, maxRange, (Assembly.CurrentSpeed.Raw - minRangeSpeed) / Assembly.MaxSpeed)
                : range;
            var show = Vector3.Distance(Assembly.FrontModule.Transform.position, MovementController.Current.LastPosition) <= currentRange;
            if (show == _previouslyShown)
                return;
            _previouslyShown = show;
            foreach (var o in gameObjects)
                o.SetActive(show);
            foreach (var r in renderers)
                r.enabled = show;
        }

    }

}

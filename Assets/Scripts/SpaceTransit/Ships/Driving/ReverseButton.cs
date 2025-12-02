using SpaceTransit.Interactions;
using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Ships.Driving
{

    public sealed class ReverseButton : ModuleComponentBase, IInteractable
    {

        [SerializeField]
        private float rotation = 90;

        private bool _wasReverse;

        public void OnInteracted()
        {
            if (Assembly.IsStationary())
                Assembly.Reverse = !Assembly.Reverse;
        }

        private void Update()
        {
            var reverse = Assembly.Reverse;
            if (_wasReverse == reverse)
                return;
            _wasReverse = reverse;
            if (reverse)
                Transform.Rotate(0, -rotation, 0);
            else
                Transform.Rotate(0, rotation, 0);
        }

    }

}

using UnityEngine;

namespace SpaceTransit.Vaulter
{

    public sealed class VaulterScreen : VaulterComponentBase
    {

        [SerializeField]
        private GameObject notInService;

        public override void OnRouteChanged()
        {
            base.OnRouteChanged();
        }

    }

}

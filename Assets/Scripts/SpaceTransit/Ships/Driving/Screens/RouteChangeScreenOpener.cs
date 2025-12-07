using SpaceTransit.Vaulter;
using UnityEngine;

namespace SpaceTransit.Ships.Driving.Screens
{

    public sealed class RouteChangeScreenOpener : VaulterComponentBase
    {

        [SerializeField]
        private ScreenSlot slot;

        public override void OnRouteChanged() => slot.Show(ScreenSlot.Slot.Exits);

    }

}

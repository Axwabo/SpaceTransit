using SpaceTransit.Interactions;
using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Ships.Driving
{

    public sealed class HoverToggleButton : ModuleComponentBase, IInteractable
    {

        public void OnInteracted()
        {
            if (Controller.CanLand)
                Controller.Land();
            else if (Controller.CanLiftOff)
                Controller.LiftOff();
        }

        public override void OnStateChanged()
        {
            if (State == ShipState.Landing)
                Transform.Translate(Vector3.down * 0.02f);
            else if (State == ShipState.LiftingOff)
                Transform.Translate(Vector3.up * 0.02f);
        }

    }

}

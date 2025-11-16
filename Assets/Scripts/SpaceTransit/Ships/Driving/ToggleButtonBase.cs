using SpaceTransit.Interactions;
using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Ships.Driving
{

    public abstract class ToggleButtonBase : ModuleComponentBase, IInteractable
    {

        public abstract void OnInteracted();

        protected void Press() => Transform.Translate(Vector3.down * 0.002f);

        protected void Release() => Transform.Translate(Vector3.up * 0.002f);

    }

}

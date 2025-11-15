using UnityEngine;

namespace SpaceTransit.Ships.Modules
{

    public abstract class ShipComponentBase : MonoBehaviour
    {

        public ShipController Controller { get; private set; }

        public ShipAssembly Assembly => Controller.Assembly;

        public void Initialize(ShipController controller)
        {
            Controller = controller;
            OnInitialized();
        }

        protected virtual void OnInitialized()
        {
        }

        public virtual void OnStateChanged(ShipState previousState)
        {
        }

    }

}

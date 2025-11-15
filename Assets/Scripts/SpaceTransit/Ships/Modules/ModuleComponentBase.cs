using UnityEngine;

namespace SpaceTransit.Ships.Modules
{

    public abstract class ModuleComponentBase : MonoBehaviour
    {

        protected Transform Transform { get; private set; }

        public ShipModule Module { get; private set; }

        public ShipAssembly Assembly => Module.Assembly;

        public ShipState State => Assembly.Controller.State;

        public void Initialize(ShipModule module)
        {
            Module = module;
            OnInitialized();
        }

        protected virtual void OnInitialized()
        {
        }

        protected virtual void Awake() => Transform = transform;

        public virtual void OnStateChanged()
        {
        }

    }

}

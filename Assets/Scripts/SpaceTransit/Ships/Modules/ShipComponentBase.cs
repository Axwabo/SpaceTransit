using UnityEngine;

namespace SpaceTransit.Ships.Modules
{

    public abstract class ShipComponentBase : MonoBehaviour
    {

        public ShipAssembly Assembly { get; private set; }

        public void Initialize(ShipAssembly assembly) => Assembly = assembly;

        public virtual void OnStateChanged()
        {
        }

    }

}

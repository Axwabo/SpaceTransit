using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Cosmos.Actions
{

    public abstract class SafetyActionBase : MonoBehaviour
    {

        public SafetyEnsurer Ensurer { get; set; }

        public virtual void OnEntering(ShipModule module)
        {
        }

        public virtual void OnEntered(ShipModule module)
        {
        }

        public virtual void OnExiting(ShipModule module)
        {
        }

        public virtual void OnExited(ShipModule module)
        {
        }

    }

}

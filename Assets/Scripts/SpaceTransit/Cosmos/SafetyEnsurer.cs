using System.Collections.Generic;
using SpaceTransit.Ships;
using SpaceTransit.Ships.Modules;
using SpaceTransit.Tubes;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    [RequireComponent(typeof(TubeBase))]
    public abstract class SafetyEnsurer : MonoBehaviour
    {

        protected HashSet<ShipModule> Occupants { get; } = new();

        protected TubeBase Tube { get; private set; }

        private void Awake() => Tube = GetComponent<TubeBase>();

        protected bool IsFreeFor(ShipAssembly assembly)
        {
            foreach (var module in Occupants)
                if (module.Assembly != assembly)
                    return false;
            return true;
        }

        public virtual bool CanProceed(ShipAssembly assembly) => IsFreeFor(assembly);

        public virtual void OnEntered(ShipModule module) => Occupants.Add(module);

        public virtual void OnExited(ShipModule module) => Occupants.Remove(module);

    }

}

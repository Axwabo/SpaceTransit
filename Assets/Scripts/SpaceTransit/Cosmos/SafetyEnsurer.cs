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

        public HashSet<ShipModule> Occupants { get; } = new();

        protected TubeBase Tube { get; private set; }

        public virtual bool IsOccupied => Occupants.Count != 0;

        private void Awake() => Tube = GetComponent<TubeBase>();

        public bool IsFreeFor(ShipAssembly assembly)
        {
            foreach (var module in Occupants)
                if (module.Assembly != assembly)
                    return false;
            return true;
        }

        public abstract bool CanProceed(ShipAssembly assembly);

        public virtual void OnEntered(ShipModule module) => Occupants.Add(module);

        public virtual void OnExited(ShipModule module) => Occupants.Remove(module);

    }

}

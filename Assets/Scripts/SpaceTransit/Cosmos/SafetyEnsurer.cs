using System.Collections.Generic;
using SpaceTransit.Cosmos.Actions;
using SpaceTransit.Ships;
using SpaceTransit.Ships.Modules;
using SpaceTransit.Tubes;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    [RequireComponent(typeof(TubeBase))]
    public abstract class SafetyEnsurer : MonoBehaviour
    {

        private SafetyActionBase[] _actions;

        public HashSet<ShipModule> Occupants { get; } = new();

        public TubeBase Tube { get; private set; }

        public bool IsOccupied => Occupants.Count != 0;

        private void Awake()
        {
            Tube = GetComponent<TubeBase>();
            _actions = GetComponents<SafetyActionBase>();
            foreach (var action in _actions)
                action.Ensurer = this;
        }

        public bool IsFreeFor(ShipAssembly assembly)
        {
            foreach (var module in Occupants)
                if (module.Assembly != assembly)
                    return false;
            return true;
        }

        public abstract bool CanProceed(ShipAssembly assembly);

        public void OnEntered(ShipModule module)
        {
            foreach (var action in _actions)
                action.OnEntering(module);
            Occupants.Add(module);
            foreach (var action in _actions)
                action.OnEntered(module);
        }

        public void OnExited(ShipModule module)
        {
            foreach (var action in _actions)
                action.OnExiting(module);
            Occupants.Remove(module);
            foreach (var action in _actions)
                action.OnExited(module);
        }

    }

}

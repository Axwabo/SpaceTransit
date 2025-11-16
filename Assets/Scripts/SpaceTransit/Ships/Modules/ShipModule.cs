using System.Linq;
using SpaceTransit.Audio;
using SpaceTransit.Movement;
using UnityEngine;

namespace SpaceTransit.Ships.Modules
{

    [RequireComponent(typeof(ModuleThruster))]
    public sealed class ShipModule : ShipComponentBase, IDepartureBlocker
    {

        [field: SerializeField]
        public ModuleAudioBounds AudioBounds { get; private set; }

        [field: SerializeField]
        public Mountable Mount { get; private set; }

        public bool CanDepart => _components.All(e => e is not IDepartureBlocker {CanDepart: false});

        public ModuleThruster Thruster { get; private set; }

        private ModuleComponentBase[] _components;

        protected override void Awake()
        {
            _components = GetComponentsInChildren<ModuleComponentBase>();
            Thruster = _components.OfType<ModuleThruster>().First();
        }

        protected override void OnInitialized()
        {
            foreach (var component in _components)
                component.Initialize(this);
        }

        public override void OnStateChanged(ShipState previousState)
        {
            foreach (var component in _components)
                component.OnStateChanged();
        }

    }

}

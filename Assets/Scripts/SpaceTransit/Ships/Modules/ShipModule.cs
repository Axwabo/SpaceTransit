using SpaceTransit.Audio;
using SpaceTransit.Movement;
using UnityEngine;

namespace SpaceTransit.Ships.Modules
{

    [RequireComponent(typeof(ModuleThruster))]
    public sealed class ShipModule : ShipComponentBase
    {

        [field: SerializeField]
        public ModuleAudioBounds AudioBounds { get; private set; }

        [field: SerializeField]
        public Mountable Mount { get; private set; }

        private ModuleComponentBase[] _components;

        private void Awake() => _components = GetComponentsInChildren<ModuleComponentBase>();

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

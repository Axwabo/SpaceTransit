using SpaceTransit.Audio;
using SpaceTransit.Movement;
using UnityEngine;

namespace SpaceTransit.Ships.Modules
{

    [RequireComponent(typeof(ModuleThruster))]
    public sealed class ShipModule : MonoBehaviour
    {

        [field: SerializeField]
        public ModuleAudioBounds AudioBounds { get; private set; }

        [field: SerializeField]
        public Mountable Mount { get; private set; }

        public ShipAssembly Assembly { get; private set; }

        private ModuleComponentBase[] _components;

        private void Awake()
        {
            _components = GetComponentsInChildren<ModuleComponentBase>();
            foreach (var component in _components)
                component.Initialize(this);
        }

        public void Initialize(ShipAssembly assembly) => Assembly = assembly;

        public void OnStateChanged()
        {
            foreach (var component in _components)
                component.OnStateChanged();
        }

    }

}

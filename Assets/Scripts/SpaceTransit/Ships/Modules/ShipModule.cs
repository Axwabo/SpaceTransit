using System;
using System.Linq;
using SpaceTransit.Audio;
using SpaceTransit.Movement;
using SpaceTransit.Ships.Driving.Screens;
using SpaceTransit.Ships.Modules.Doors;
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

        public bool CanDepart => _components.All(static e => e is not IDepartureBlocker {CanDepart: false});

        public ReadOnlySpan<DoorController> Doors => _doors;

        public ModuleThruster Thruster { get; private set; }

        private ModuleComponentBase[] _components;

        private DoorController[] _doors;

        protected override void Awake()
        {
            base.Awake();
            _components = GetComponentsInChildren<ModuleComponentBase>();
            Thruster = _components.OfType<ModuleThruster>().First();
            _doors = _components.OfType<DoorController>().ToArray();
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

        public EntryList EntryList => _components.OfType<EntryList>().First();

        public ExitList ExitList => _components.OfType<ExitList>().First();

    }

}

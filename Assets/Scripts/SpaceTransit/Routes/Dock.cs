using System.Collections.Generic;
using SpaceTransit.Cosmos;
using SpaceTransit.Cosmos.Actions;
using SpaceTransit.Ships;
using SpaceTransit.Ships.Modules;
using SpaceTransit.Tubes;
using UnityEngine;

namespace SpaceTransit.Routes
{

    public sealed class Dock : StraightTube
    {

        [field: SerializeField]
        public Exit[] FrontExits { get; private set; }

        [field: SerializeField]
        public Exit[] BackExits { get; private set; }

        [field: SerializeField]
        public Entry[] FrontEntries { get; private set; }

        [field: SerializeField]
        public Entry[] BackEntries { get; private set; }

        [field: SerializeField]
        public bool Left { get; private set; }

        [field: SerializeField]
        public bool Right { get; private set; }

        public Station Station { get; set; }

        public int Index { get; set; }

        public HashSet<ShipAssembly> UsedBy { get; } = new();

        protected override void Awake()
        {
            base.Awake();
            if (!Application.isPlaying)
                return;
            foreach (var entry in FrontEntries)
                entry.Dock = this;
            foreach (var entry in BackEntries)
                entry.Dock = this;
        }

        protected override SafetyEnsurer AddDefaultSafety(GameObject o)
        {
            o.AddComponent<ExitLightHandler>().Dock = this;
            return o.AddComponent<DockSafety>();
        }

        private sealed class ExitLightHandler : SafetyActionBase
        {

            public Dock Dock { get; set; }

            public override void OnExited(ShipModule module)
            {
                base.OnExited(module);
            }

        }

    }

}

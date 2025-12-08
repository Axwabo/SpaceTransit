using SpaceTransit.Routes;
using SpaceTransit.Ships;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    public sealed class Entry : EntryOrExit
    {

        [SerializeField]
        private EntryEnsurer ensurer;

        public Dock Dock { get; set; }

        protected override void Awake()
        {
            base.Awake();
            ensurer.Entries.Add(this);
        }

        public bool IsUsedOnlyBy(ShipAssembly assembly) => Locks.AreOnlyUsedBy(assembly);

        public override bool Lock(ShipAssembly assembly) => !Dock.Tube.Safety.IsOccupied && base.Lock(assembly);

    }

}

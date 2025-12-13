using SpaceTransit.Routes;
using SpaceTransit.Ships;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpaceTransit.Cosmos
{

    public sealed class Entry : EntryOrExit
    {

        [field: FormerlySerializedAs("ensurer")]
        [field: SerializeField]
        public EntryEnsurer Ensurer { get; private set; }

        public Dock Dock { get; set; }

        public override bool IsFree => !Dock.Safety.IsOccupied && base.IsFree;

        protected override void Awake()
        {
            base.Awake();
            Ensurer.Entries.Add(this);
        }

        public bool IsUsedOnlyBy(ShipAssembly assembly) => Locks.AreOnlyUsedBy(assembly);

        public override bool Lock(ShipAssembly assembly) => !Dock.Safety.IsOccupied && base.Lock(assembly);

    }

}

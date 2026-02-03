using SpaceTransit.Loader;
using SpaceTransit.Routes;
using SpaceTransit.Ships;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    public sealed class Entry : EntryOrExit
    {

        [SerializeField]
        [HideInInspector]
        private string ensurerReference;

        [field: SerializeField]
        public EntryEnsurer Ensurer { get; private set; }

        public Dock Dock { get; set; }

        public override bool IsFree => !Dock.Safety.IsOccupied && base.IsFree;

        protected override void Awake()
        {
            base.Awake();
            RefreshEnsurer();
            CrossSceneObject.SubscribeToSceneChanges(RefreshEnsurer, ensurerReference);
        }

        private void OnValidate() => ensurerReference = CrossSceneObject.GetOrCreate(Ensurer, gameObject, ensurerReference);

        private void OnDestroy() => CrossSceneObject.ScenesChanged -= RefreshEnsurer;

        private void RefreshEnsurer()
        {
            Ensurer = CrossSceneObject.GetComponent(ensurerReference, Ensurer);
            if (Ensurer)
                Ensurer.Entries.Add(this);
        }

        public override bool Lock(ShipAssembly assembly) => !Dock.Safety.IsOccupied && base.Lock(assembly);

    }

}

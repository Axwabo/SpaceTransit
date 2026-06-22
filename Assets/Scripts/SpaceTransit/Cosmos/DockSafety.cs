using SpaceTransit.Loader;
using SpaceTransit.Routes;
using SpaceTransit.Ships;
using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    [RequireComponent(typeof(Dock))]
    public sealed class DockSafety : NextSegmentSafety
    {

        [SerializeField]
        [HideInInspector]
        private string[] lockReferences;

        [SerializeField]
        private Lock[] locks;

        private Dock Dock => (Dock) Tube;

        private void Start()
        {
            RefreshLocks();
            CrossSceneObject.SubscribeToSceneChanges(RefreshLocks, lockReferences);
        }

        private void OnValidate() => lockReferences = CrossSceneObject.GetOrCreateAll(locks, gameObject, lockReferences);

        private void OnDestroy() => CrossSceneObject.ScenesChanged -= RefreshLocks;

        private void RefreshLocks() => locks = CrossSceneObject.GetAllComponents(lockReferences, locks);

        public override bool CanProceed(ShipAssembly assembly)
        {
            if (locks != null && !locks.CanClaim(assembly))
                return false;
            if (!assembly.IsStationary())
                return base.CanProceed(assembly);
            if (assembly.Reverse ? !Tube.HasPrevious : !Tube.HasNext)
                return false;
            var exits = assembly.Reverse ? Dock.BackExits : Dock.FrontExits;
            if (exits.Length == 0)
                return base.CanProceed(assembly);
            foreach (var exit in exits)
                if (exit.IsUsedOnlyBy(assembly))
                    return base.CanProceed(assembly);
            return false;
        }

        public override void OnEntered(ShipModule module)
        {
            base.OnEntered(module);
            if (Occupants.Count != module.Assembly.Modules.Length)
                return;
            var exits = module.Assembly.Reverse ? Dock.FrontExits : Dock.BackExits;
            foreach (var exit in exits)
                if (exit.Clearance)
                    exit.Clearance.Release(module.Assembly);
        }

    }

}

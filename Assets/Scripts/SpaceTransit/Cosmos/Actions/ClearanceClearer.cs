using SpaceTransit.Loader;
using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Cosmos.Actions
{

    public sealed class ClearanceClearer : SafetyActionBase
    {

        [SerializeField]
        [HideInInspector]
        private string clearanceReference;

        [SerializeField]
        private OpposingTrafficClearance clearance;

        private void Start()
        {
            RefreshClearance();
            CrossSceneObject.SubscribeToSceneChanges(RefreshClearance, clearanceReference);
        }

        private void OnValidate() => clearanceReference = CrossSceneObject.GetOrCreate(clearance, gameObject, clearanceReference);

        private void RefreshClearance() => clearance = CrossSceneObject.GetComponent(clearanceReference, clearance);

        public override void OnExited(ShipModule module)
        {
            if (Ensurer.Occupants.Count == 0)
                clearance.Release(module.Assembly);
        }

    }

}

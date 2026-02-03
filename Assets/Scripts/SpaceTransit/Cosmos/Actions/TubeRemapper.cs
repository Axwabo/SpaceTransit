using SpaceTransit.Loader;
using SpaceTransit.Ships.Modules;
using SpaceTransit.Tubes;
using UnityEngine;

namespace SpaceTransit.Cosmos.Actions
{

    public sealed class TubeRemapper : SafetyActionBase
    {

        [SerializeField]
        [HideInInspector]
        private string tubeReference;

        [SerializeField]
        [HideInInspector]
        private string toReference;

        [SerializeField]
        public TubeBase connectTube;

        [SerializeField]
        public TubeBase connectTo;

        private void Start()
        {
            RefreshTubes();
            CrossSceneObject.SubscribeToSceneChanges(RefreshTubes, tubeReference, toReference);
        }

        private void OnValidate()
        {
            var go = gameObject;
            tubeReference = CrossSceneObject.GetOrCreate(connectTube, go);
            toReference = CrossSceneObject.GetOrCreate(connectTo, go);
        }

        private void OnDestroy() => CrossSceneObject.ScenesChanged -= RefreshTubes;

        private void RefreshTubes()
        {
            connectTube = CrossSceneObject.GetComponent(tubeReference, connectTube);
            connectTo = CrossSceneObject.GetComponent(toReference, connectTo);
        }

        public override void OnEntered(ShipModule module)
        {
            if (Ensurer.Occupants.Count == 1)
                Remap();
        }

        public void Remap() => connectTube.SetNext(connectTo);

    }

}

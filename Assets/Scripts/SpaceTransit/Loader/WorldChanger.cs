using UnityEngine;

namespace SpaceTransit.Loader
{

    public sealed class WorldChanger : MonoBehaviour
    {

        [SerializeField]
        private int unloadForwards;

        [SerializeField]
        private int unloadBackwards;

        [SerializeField]
        private int loadForwards;

        [SerializeField]
        private int loadBackwards;

        private void OnTriggerEnter(Collider other)
        {
            var t = transform;
            var isBack = Vector3.Dot(t.InverseTransformPoint(other.transform.position).normalized, t.forward) < 0;
            World.Unload(isBack ? unloadForwards : unloadBackwards);
            _ = World.Load(isBack ? loadForwards : loadBackwards);
        }

    }

}

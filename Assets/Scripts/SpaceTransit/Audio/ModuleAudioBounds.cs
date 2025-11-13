using UnityEngine;

namespace SpaceTransit.Audio
{

    public sealed class ModuleAudioBounds : MonoBehaviour
    {

        [SerializeField]
        private new Collider collider; // unity can you just remove these stupid properties

        public Vector3 Closest(Vector3 position) => collider.ClosestPoint(position);

    }

}

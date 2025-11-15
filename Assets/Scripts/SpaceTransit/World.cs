using UnityEngine;

namespace SpaceTransit
{

    public sealed class World : MonoBehaviour
    {

        public const float MetersToWorld = 0.1f;
        public const float WorldToMeters = 10;

        public static Transform Current { get; private set; }

        private void Awake() => Current = transform;

    }

}

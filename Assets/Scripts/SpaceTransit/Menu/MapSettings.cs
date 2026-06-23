using UnityEngine;

namespace SpaceTransit.Menu
{

    [CreateAssetMenu(fileName = "Map Settings", menuName = "SpaceTransit/Map Settings")]
    public sealed class MapSettings : ScriptableObject
    {

        public int minX;

        public int minY;

        public int width = 8192;

        public int height = 4096;

        public Vector3 Center => new(minX + width * 0.5f, 0, minY + height * 0.5f);

        public Vector3 Size => new(width, 1, height);

    }

}

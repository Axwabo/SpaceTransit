using System.Collections.Generic;
using SpaceTransit.Routes;
using UnityEngine;

namespace SpaceTransit
{

    public sealed class World : MonoBehaviour
    {

        public const float MetersToWorld = 0.1f;
        public const float WorldToMeters = 10;

        private static readonly HashSet<RouteDescriptor> RouteCache = new();

        public static IReadOnlyCollection<RouteDescriptor> Routes => RouteCache;

        [SerializeField]
        private RouteDescriptor[] routes;

        public static Transform Current { get; private set; }

        private void Awake()
        {
            Current = transform;
            RouteCache.UnionWith(routes);
        }

        private void OnDestroy() => RouteCache.ExceptWith(routes);

    }

}

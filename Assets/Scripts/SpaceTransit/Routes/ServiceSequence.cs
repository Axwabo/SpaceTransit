using SpaceTransit.Vaulter;
using UnityEngine;

namespace SpaceTransit.Routes
{

    [CreateAssetMenu(fileName = "Service Sequence", menuName = "SpaceTransit/Service Sequence")]
    public sealed class ServiceSequence : ScriptableObject
    {

        [SerializeField]
        public VaulterController prefab;

        [SerializeField]
        public RouteDescriptor[] routes;

    }

}

using UnityEngine;

namespace SpaceTransit.Routes
{

    public sealed class RouteDescriptor : MonoBehaviour
    {

        [SerializeField]
        private TimeOnly departure;

        [SerializeField]
        private bool reverse;

    }

}

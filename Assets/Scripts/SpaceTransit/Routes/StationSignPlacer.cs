using SpaceTransit.Tubes;
using Unity.Mathematics;
using UnityEngine;

namespace SpaceTransit.Routes
{

    [RequireComponent(typeof(Station))]
    public sealed class StationSignPlacer : MonoBehaviour
    {

        [SerializeField]
        private TubeBase forwards;

        [SerializeField]
        private TubeBase backwards;

        [SerializeField]
        private float distance = 1050;

        private void Start()
        {
            var station = GetComponent<Station>();
            if (!forwards)
                forwards = station.Docks[^1].Tube.Next;

            var remaining = distance;
            var tube = forwards;
            while (remaining > 0)
            {
                if (remaining > tube.Length || !tube.HasNext)
                    break;
                remaining -= tube.Length;
                tube = tube.Next;
            }

            var (position, rotation) = tube.Sample(Mathf.Clamp(remaining, 0, tube.Length));
            Instantiate(World.StationSignPrefab, tube.Transform).SetLocalPositionAndRotation(position, rotation * quaternion.Euler(0, 180, 0));
            // TODO backwards
        }

    }

}

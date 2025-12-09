using SpaceTransit.Tubes;
using UnityEngine;

namespace SpaceTransit.Routes
{

    [RequireComponent(typeof(Station))]
    public sealed class StationSignPlacer : MonoBehaviour
    {

        [SerializeField]
        private TubeBase origin;

        [SerializeField]
        private bool backwards;

        [SerializeField]
        private float distance = 1000;

        private void Start()
        {
            var station = GetComponent<Station>();
            if (!origin)
            {
                origin = station.Docks[backwards ? ^1 : 0];
                if (backwards ? !origin.HasPrevious : !origin.HasNext)
                    return;
                origin = backwards ? origin.Previous : origin.Next;
            }

            var (tube, remaining) = backwards ? GetPlacementBackwards() : GetPlacementForwards();
            PlaceSign(tube, remaining);
        }

        private (TubeBase Tube, float Remaining) GetPlacementForwards()
        {
            var current = 0f;
            var tube = origin;
            while (current < distance * World.MetersToWorld && distance * World.MetersToWorld - current > tube.Length)
            {
                if (!tube.HasNext)
                    break;
                current += tube.Length;
                tube = tube.Next;
            }

            return (tube, distance - current);
        }

        private (TubeBase Tube, float Remaining) GetPlacementBackwards()
        {
            var current = 0f;
            var tube = origin;
            while (current < distance * World.MetersToWorld && distance * World.MetersToWorld - current > tube.Length)
            {
                if (!tube.HasPrevious)
                    break;
                current += tube.Length;
                tube = tube.Previous;
            }

            return (tube, distance - current);
        }

        private void PlaceSign(TubeBase tube, float remaining)
        {
            var (position, rotation) = tube.Sample(Mathf.Clamp(remaining, 0, tube.Length));
            var clone = Instantiate(World.StationSignPrefab, tube.Transform);
            if (!backwards)
                rotation *= Quaternion.Euler(0, 180, 0);
            clone.SetLocalPositionAndRotation(position, rotation);
        }

    }

}

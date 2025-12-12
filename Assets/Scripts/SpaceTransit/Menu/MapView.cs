using SpaceTransit.Routes;
using UnityEngine;

namespace SpaceTransit.Menu
{

    public sealed class MapView : MonoBehaviour
    {

        [SerializeField]
        private MapStation prefab;

        [SerializeField]
        private RectTransform anchor;

        private bool _placed;

        private void Start()
        {
            if (_placed)
                return;
            var parent = transform;
            foreach (var station in Station.LoadedStations)
            {
                var t = new GameObject(station.Name).transform;
                var stationPosition = station.transform.localPosition;
                t.parent = anchor;
                t.localPosition = new Vector3(stationPosition.x * World.MetersToWorld, stationPosition.z * World.MetersToWorld);
                Instantiate(prefab, parent).Apply(station, t);
            }
        }

    }

}

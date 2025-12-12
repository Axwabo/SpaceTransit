using SpaceTransit.Routes;
using UnityEngine;

namespace SpaceTransit.Menu
{

    public sealed class MapView : MonoBehaviour
    {

        [SerializeField]
        private MapStation prefab;

        [SerializeField]
        private RectTransform constraints;

        [SerializeField]
        private Transform anchor;

        [SerializeField]
        private float scale = 1;

        private bool _placed;

        private void Start()
        {
            if (_placed)
                return;
            var parent = transform;
            foreach (var station in Station.LoadedStations)
            {
                var t = new GameObject(station.Name).transform;
                var stationPosition = anchor.InverseTransformPoint(station.transform.localPosition);
                t.parent = constraints;
                t.localPosition = new Vector3(stationPosition.x * scale, stationPosition.z * scale);
                Instantiate(prefab, parent).Apply(station, t);
            }
        }

    }

}

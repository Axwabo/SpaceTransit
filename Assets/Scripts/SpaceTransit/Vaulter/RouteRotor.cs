using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Ships;
using UnityEngine;

namespace SpaceTransit.Vaulter
{

    public sealed class RouteRotor : MonoBehaviour
    {

        [SerializeField]
        private VaulterController prefab;

        [SerializeField]
        public RouteDescriptor[] routes;

        private RouteDescriptor _initial;

        private int _startingStop;

        private int _index;

        private VaulterController _ship;

        private float _delay;

        private void Awake()
        {
            if (routes.Length == 0)
                Destroy(this);
        }

        private void Start()
        {
            for (var i = 0; i < routes.Length; i++)
            {
                var route = routes[i];
                if (route.Origin.Departure > Clock.Now)
                {
                    Spawn(route);
                    return;
                }

                _index = i;
                for (var j = 0; j < route.IntermediateStops.Length; j++)
                {
                    var stop = route.IntermediateStops[j];
                    if (stop.Arrival < Clock.Now)
                        continue;
                    Spawn(route);
                    _ship.initialStopIndex = j;
                    return;
                }
            }

            Destroy(this);
        }

        private void Spawn(RouteDescriptor route)
        {
            _ship = Instantiate(prefab, World.Current);
            _ship.initialRoute = route;
        }

        private void Update()
        {
            if (_ship.Stop is not Destination || _ship.Parent.State != ShipState.Docked || !_ship.Assembly.IsStationary() || _ship.Assembly.IsManuallyDriven)
                return;
            if (_delay <= 0)
                _delay = 60;
            else if ((_delay -= Clock.Delta) <= 0 && ++_index < routes.Length)
                _ship.BeginRoute(routes[_index]);
        }

    }

}

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
        private RouteDescriptor[] routes;

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
            _ship = Instantiate(prefab, World.Current);
            _ship.initialRoute = routes[0];
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

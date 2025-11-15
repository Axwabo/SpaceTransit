using SpaceTransit.Routes;
using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Vaulter
{

    public abstract class VaulterComponentBase : SubcomponentBase<VaulterController>
    {

        public bool IsInService => Parent.IsInService;

        public bool IsTerminus => Parent.Stop == Parent.Route.Destination;

        protected bool IsNearStation => Station.TryGetLoadedStation(Parent.Stop.Station, out var station)
                                        && Vector3.Distance(
                                            station.Docks[Parent.Stop.DockIndex].transform.position,
                                            Parent.Assembly.FrontModule.transform.position
                                        ) < 100; // TODO: optimize position getters

        public virtual void OnRouteChanged()
        {
        }

        public virtual void OnStopChanged()
        {
        }

    }

}

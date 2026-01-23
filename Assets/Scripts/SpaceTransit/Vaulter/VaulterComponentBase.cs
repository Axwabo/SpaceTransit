using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Ships;
using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Vaulter
{

    public abstract class VaulterComponentBase : SubcomponentBase<VaulterController>
    {

        protected ShipController Controller => Parent.Parent;

        protected ShipAssembly Assembly => Parent.Assembly;

        protected bool IsInService => Parent.IsInService;

        protected bool IsTerminus => Parent.Stop is Destination;

        protected bool IsNearStation => Station.TryGetLoadedStation(Parent.Stop.Station, out var station)
                                        && Vector3.Distance(
                                            station.Docks[Parent.Stop.DockIndex].transform.position,
                                            FrontPosition
                                        ) < 20f;

        protected Vector3 FrontPosition => Parent.Assembly.FrontModule.Transform.position;

        public virtual void OnRouteChanged()
        {
        }

        public virtual void OnStopChanged()
        {
        }

    }

}

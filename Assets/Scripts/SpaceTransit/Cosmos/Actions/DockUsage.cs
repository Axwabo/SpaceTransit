using SpaceTransit.Routes;
using SpaceTransit.Ships.Modules;

namespace SpaceTransit.Cosmos.Actions
{

    public sealed class DockUsage : SafetyActionBase
    {

        private Dock Dock => (Dock) Ensurer.Tube;

        public override void OnEntering(ShipModule module)
        {
            if (!Ensurer.IsOccupied)
                Dock.UsedBy.Add(module.Assembly);
        }

        public override void OnExited(ShipModule module)
        {
            if (!Ensurer.IsOccupied)
                Dock.UsedBy.Remove(module.Assembly);
        }

    }

}

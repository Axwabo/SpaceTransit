using SpaceTransit.Interactions;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Ships.Modules;

namespace SpaceTransit.Ships.Driving
{

    public sealed class RestartButton : ModuleComponentBase, IInteractable
    {

        private bool VaulterAllowsRestart => !Controller.TryGetVaulter(out var vaulter) || !vaulter.IsInService || vaulter.Target is Origin or Destination;

        public void OnInteracted()
        {
            if (!Controller.IsRestarting && Controller.State == ShipState.Docked && VaulterAllowsRestart)
                _ = Controller.RestartAsync();
        }

    }

}

using SpaceTransit.Interactions;
using SpaceTransit.Ships.Modules;

namespace SpaceTransit.Ships.Driving
{

    public sealed class RestartButton : ModuleComponentBase, IInteractable
    {

        public void OnInteracted()
        {
            if (!Controller.IsRestarting)
                _ = Controller.RestartAsync();
        }

    }

}

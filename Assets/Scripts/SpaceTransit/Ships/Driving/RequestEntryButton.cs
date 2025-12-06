using SpaceTransit.Cosmos;
using SpaceTransit.Interactions;
using SpaceTransit.Ships.Modules;
using UnityEditor;

namespace SpaceTransit.Ships.Driving
{

    public sealed class RequestEntryButton : ModuleComponentBase, IInteractable
    {

        // TODO: refactor
        public void OnInteracted()
        {
            if (Assembly.FrontModule.Thruster.Tube.Safety is not EntryEnsurer {station: var station})
                return;
            var dock = station.Docks[0];
            var entry = Assembly.Reverse ? dock.FrontEntry : dock.BackEntry;
            if (entry && !entry.Lock(Assembly))
                EditorUtility.DisplayDialog("lock failed", "Couldn't request entry", "kurwa");
        }

    }

}

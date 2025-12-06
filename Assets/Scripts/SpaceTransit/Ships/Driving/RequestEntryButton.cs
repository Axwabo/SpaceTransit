using SpaceTransit.Interactions;
using SpaceTransit.Ships.Modules;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SpaceTransit.Ships.Driving
{

    public sealed class RequestEntryButton : ModuleComponentBase, IInteractable
    {

        [SerializeField]
        private DockList list;

        public void OnInteracted()
        {
            if (list.Selected is not (not -1 and var selected))
                return;
            var dock = list.TowardsStation.Docks[selected];
            var entry = Assembly.Reverse ? dock.FrontEntry : dock.BackEntry;
            // TODO: refactor
            if (entry && !entry.Lock(Assembly))
#if UNITY_EDITOR
                EditorUtility.DisplayDialog("lock failed", "Couldn't request entry", "kurwa");
#else
                ;
#endif
        }

    }

}

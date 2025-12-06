using SpaceTransit.Interactions;
using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Ships.Driving
{

    public sealed class RequestEntryButton : ModuleComponentBase, IInteractable
    {

        [SerializeField]
        private DockList list;

        public void OnInteracted() => list.Enter(list.Selected);

    }

}

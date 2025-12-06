using SpaceTransit.Interactions;
using UnityEngine;

namespace SpaceTransit.Ships.Driving
{

    public sealed class UpButton : MonoBehaviour, IInteractable
    {

        [SerializeField]
        private DockList list;

        [SerializeField]
        private bool down;

        public void OnInteracted() => list.Select(down);

    }

}

using SpaceTransit.Movement;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Menu
{

    public sealed class InteractButton : MonoBehaviour
    {

        private void Start() => this.RootVisual().Q("Interact").RegisterCallback<PointerDownEvent>(_ => TouchscreenMode.Interact = true);

    }

}

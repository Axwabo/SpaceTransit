using SpaceTransit.Movement;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Menu
{

    public sealed class InteractButton : MonoBehaviour
    {

        private void Start()
        {
            var button = this.RootVisual().Q<Button>("Interact");
            button.RegisterCallback<MouseDownEvent>(_ => TouchscreenMode.Interact = true);
            button.RegisterCallback<PointerDownEvent>(_ => TouchscreenMode.Interact = true);
        }

    }

}

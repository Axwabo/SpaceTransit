using SpaceTransit.Movement;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Menu
{

    public sealed class JumpButton : MonoBehaviour
    {

        private void Start()
        {
            var button = this.RootVisual().Q<Button>("Jump");
            button.RegisterCallback<PointerDownEvent>(_ => TouchscreenMode.Jump = true);
            button.RegisterCallback<PointerUpEvent>(_ => TouchscreenMode.Jump = false);
        }

    }

}

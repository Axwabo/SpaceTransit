using SpaceTransit.Movement;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Menu.Main
{

    public sealed class TouchscreenToggle : MonoBehaviour
    {

        private Toggle _toggle;

        private void Start()
        {
            _toggle = this.RootVisual().Q<Toggle>("Touchscreen");
            _toggle.value = TouchscreenMode.Enabled;
            _toggle.RegisterValueChangedCallback(evt => TouchscreenMode.Enabled = evt.newValue);
        }

    }

}

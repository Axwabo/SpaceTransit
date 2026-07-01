using SpaceTransit.Movement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace SpaceTransit.Menu
{

    public sealed class MenuScreen : MonoBehaviour
    {

        public static bool IsOpen { get; private set; }

        public static MenuScreen Current { get; private set; }

        private static GameObject _current;

        private VisualElement _root;

        [SerializeField]
        private PanelToggleButton[] toggles;

        private void Start()
        {
            _current = gameObject;
            Current = this;
            IsOpen = true;
            _root = this.RootVisual();
            Toggle();
        }

        private void Update()
        {
            if (InputSystem.actions["Menu"].WasPressedThisFrame())
                Toggle();
        }

        private void OnDestroy()
        {
            IsOpen = false;
            _current = null;
            Current = null;
        }

        public void Toggle()
        {
            _root.SetVisibility(IsOpen = !IsOpen);
            Cursor.lockState = IsOpen || TouchscreenMode.Enabled ? CursorLockMode.None : CursorLockMode.Locked;
            if (IsOpen)
                return;
            foreach (var panel in toggles)
                panel.SetVisibility(false);
        }

        public static void Disable() => _current.SetActive(false);

    }

}

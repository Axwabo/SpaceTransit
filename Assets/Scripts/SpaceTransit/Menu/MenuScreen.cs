using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace SpaceTransit.Menu
{

    public sealed class MenuScreen : MonoBehaviour
    {

        public static bool IsOpen { get; private set; }

        private static GameObject _current;

        private VisualElement _root;

        private void Start()
        {
            _current = gameObject;
            IsOpen = true;
            _root = this.RootVisual();
            Toggle();
        }

        private void Update()
        {
            if (InputSystem.actions["Menu"].WasPressedThisFrame())
                Toggle();
        }

        private void OnDestroy() => IsOpen = false;

        private void Toggle()
        {
            _root.SetVisibility(IsOpen = !IsOpen);
            Cursor.lockState = IsOpen ? CursorLockMode.None : CursorLockMode.Locked;
        }

        public static void Disable() => _current.SetActive(false);

    }

}

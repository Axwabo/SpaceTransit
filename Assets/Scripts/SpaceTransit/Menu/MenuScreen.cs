using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceTransit.Menu
{

    public sealed class MenuScreen : MonoBehaviour
    {

        public static bool IsOpen { get; private set; }

        private static GameObject _current;

        [SerializeField]
        private GameObject ui;

        private void Start()
        {
            _current = gameObject;
            IsOpen = false;
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
            ui.SetActive(IsOpen = !ui.activeSelf);
            Cursor.lockState = IsOpen ? CursorLockMode.None : CursorLockMode.Locked;
        }

        public static void Disable() => _current.SetActive(false);

    }

}

using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceTransit.Menu
{

    public sealed class MenuScreen : MonoBehaviour
    {

        public static bool IsOpen { get; private set; }

        [SerializeField]
        private GameObject ui;

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

    }

}

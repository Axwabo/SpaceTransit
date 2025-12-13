using UnityEngine;
using UnityEngine.UI;

namespace SpaceTransit.Menu
{

    [RequireComponent(typeof(Button))]
    public sealed class AutoHideGui : AutoRegisterButton
    {

        [SerializeField]
        private GameObject target;

        protected override void Click() => target.SetActive(!target.activeSelf);

        private void OnDisable() => target.SetActive(false);

    }

}

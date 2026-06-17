using UnityEngine;

namespace SpaceTransit.Menu
{

    public sealed class PanelToggleButton : AutoRegisterButton
    {

        [SerializeField]
        private GameObject target;

        protected override void Click()
        {
            var deactivateTarget = target.activeSelf;
            gameObject.SetActive(deactivateTarget);
            target.SetActive(!deactivateTarget);
        }

    }

}

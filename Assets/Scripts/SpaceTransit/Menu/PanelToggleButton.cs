using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Menu
{

    public sealed class PanelToggleButton : AutoRegisterButton
    {

        private VisualElement _this;

        [SerializeField]
        private UIDocument target;

        [SerializeField]
        private MonoBehaviour behavior;

        [SerializeField]
        private bool ignoreThis;

        [SerializeField]
        private bool hideThisByDefault;

        protected override void Start()
        {
            base.Start();
            _this = ignoreThis ? null : this.RootVisual();
            if (hideThisByDefault)
                _this?.SetVisibility(false);
        }

        protected override void Click()
        {
            var showTarget = ignoreThis
                ? target.rootVisualElement.style.display != DisplayStyle.Flex
                : _this.style.display != DisplayStyle.None;
            _this?.SetVisibility(!showTarget);
            SetTargetVisibility(showTarget);
        }

        public void SetTargetVisibility(bool showTarget)
        {
            if (target)
                target.rootVisualElement.SetVisibility(showTarget);
            if (behavior)
                behavior.enabled = showTarget;
        }

    }

}

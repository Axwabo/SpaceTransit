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
        private bool hideThisByDefault;

        protected override void Start()
        {
            base.Start();
            _this = this.RootVisual();
            if (hideThisByDefault)
                _this.SetVisibility(false);
        }

        protected override void Click()
        {
            var showTarget = _this.style.display != DisplayStyle.None;
            _this.SetVisibility(!showTarget);
            if (target)
                target.rootVisualElement.SetVisibility(showTarget);
        }

    }

}

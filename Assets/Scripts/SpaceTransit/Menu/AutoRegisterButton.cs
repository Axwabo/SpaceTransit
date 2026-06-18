using UnityEngine;
using UnityEngine.UIElements;
using UGUIButton = UnityEngine.UI.Button;
using ToolkitButton = UnityEngine.UIElements.Button;

namespace SpaceTransit.Menu
{

    public abstract class AutoRegisterButton : MonoBehaviour
    {

        [SerializeField]
        private string buttonName;

        private UGUIButton _ugui;

        protected ToolkitButton _toolkit;

        protected virtual void Start()
        {
            if (TryGetComponent(out _ugui))
            {
                _ugui.onClick.AddListener(Click);
                return;
            }

            if (string.IsNullOrEmpty(buttonName))
                return;
            _toolkit = this.RootVisual().Q<ToolkitButton>(buttonName);
            _toolkit.clicked += Click;
        }

        private void OnDestroy()
        {
            if (_ugui)
                _ugui.onClick.RemoveListener(Click);
            else if (_toolkit != null)
                _toolkit.clicked -= Click;
        }

        protected abstract void Click();

    }

}

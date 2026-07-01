using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Menu
{

    public abstract class AutoRegisterButton : MonoBehaviour
    {

        [SerializeField]
        private string buttonName;

        protected Button Toolkit;

        protected virtual void Start()
        {
            if (string.IsNullOrEmpty(buttonName))
                return;
            Toolkit = this.RootVisual().Q<Button>(buttonName);
            Toolkit.clicked += Click;
        }

        private void OnDestroy()
        {
            if (Toolkit != null)
                Toolkit.clicked -= Click;
        }

        protected abstract void Click();

    }

}

using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Driving.Screens
{

    public abstract class ScreenBase : MonoBehaviour, ICullingListener
    {

        private VisualElement _root;

        private void OnEnable() => _root?.SetVisibility(true);

        private void OnDisable() => _root?.SetVisibility(false);

        public void Initialize()
        {
            if (_root == null)
                Initialize(_root = this.RootVisual());
        }

        protected virtual void Initialize(VisualElement root)
        {
        }

        public abstract void Navigate(bool forwards);

        public abstract void Confirm();

        public virtual bool Select(int index) => false;

        public abstract void SetVisibility(bool visible);

    }

}

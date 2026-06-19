using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Driving.Screens
{

    public abstract class ScreenBase : MonoBehaviour, ICullingListener
    {

        private VisualElement _root;

        private void OnEnable() => _root?.SetVisibility(true);

        private void OnDisable() => _root?.SetVisibility(false);

        public void Initialize() => Initialize(_root = this.RootVisual());

        protected virtual void Initialize(VisualElement root)
        {
        }

        public abstract void Navigate(bool forwards);

        public abstract void Confirm();

        public abstract bool Select(int index);

        public abstract void SetVisibility(bool visible);

    }

}

using SpaceTransit.Ships.Modules.Displays;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Driving.Screens
{

    public sealed class CosmosScreen : ModuleUIComponent, ICullingListener
    {

        [SerializeField]
        private EntryListManager dockList;

        [SerializeField]
        private ExitListManager exitList;

        private Label _text;

        private ScreenBase _current;

        private VisualElement _root;

        private bool _exitsShown = true;

        private void OnEnable()
        {
            if (_root == null)
                return;
            _root.SetVisibility(true);
            dockList.enabled = true;
            exitList.enabled = true;
        }

        private void OnDisable()
        {
            _root?.SetVisibility(false);
            dockList.enabled = false;
            exitList.enabled = false;
        }

        protected override void Initialize(VisualElement root)
        {
            _root = root;
            _text = root.Q<Label>("Title");
        }

        public void Select(int index)
        {
            _current.Select(index);
            _current.Confirm();
        }

    }

}

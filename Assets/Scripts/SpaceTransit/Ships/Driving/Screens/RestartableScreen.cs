using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Driving.Screens
{

    public sealed class RestartableScreen
    {

        private readonly VisualElement _root;
        private readonly VisualElement _main;
        private readonly VisualElement _restarting;
        private readonly ProgressBar _progress;

        public RestartableScreen(VisualElement root)
        {
            _root = root;
            _main = root.Q<VisualElement>("Main");
            _restarting = root.Q<VisualElement>("Restarting");
            _progress = root.Q<ProgressBar>("Loading");
        }

        public float Progress
        {
            set => _progress.value = value;
        }

        public void BeginRestart()
        {
            Progress = 0;
            _main.SetVisibility(false);
            _restarting.SetVisibility(false);
        }

        public void EndRestart()
        {
            _main.SetVisibility(true);
            _restarting.SetVisibility(false);
        }

        public void SetProgressVisibility(bool visible) => _restarting.SetVisibility(visible);

        public void SetClass(string className, bool enabled) => _root.EnableInClassList(className, enabled);

    }

}

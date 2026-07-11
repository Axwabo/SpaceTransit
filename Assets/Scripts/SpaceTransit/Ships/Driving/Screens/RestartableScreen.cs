using Unity.Properties;

namespace SpaceTransit.Ships.Driving.Screens
{

    public sealed class RestartableScreen
    {

        [CreateProperty]
        public float Progress { get; set; }

        [CreateProperty]
        public bool Main { get; set; }

        [CreateProperty]
        public bool Restarting { get; set; }

        public void BeginRestart()
        {
            Progress = 0;
            Main = Restarting = false;
        }

        public void EndRestart()
        {
            Main = true;
            Restarting = false;
            _main.SetVisibility(true);
            _restarting.SetVisibility(false);
        }

        public void SetProgressVisibility(bool visible) => Restarting = (visible);

        public void SetClass(string className, bool enabled) => _root.EnableInClassList(className, enabled);

    }

}

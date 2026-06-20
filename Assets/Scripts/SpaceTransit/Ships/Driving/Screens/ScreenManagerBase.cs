using SpaceTransit.Ships.Modules;

namespace SpaceTransit.Ships.Driving.Screens
{

    public abstract class ScreenManagerBase<T> : ModuleComponentBase, ICullingListener where T : ScreenBase
    {

        protected T Screen { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Screen = GetComponent<T>();
        }

        protected virtual void OnEnable() => Screen.enabled = true;

        protected virtual void OnDisable() => Screen.enabled = false;

        protected override void OnInitialized() => Screen.Initialize();

    }

}

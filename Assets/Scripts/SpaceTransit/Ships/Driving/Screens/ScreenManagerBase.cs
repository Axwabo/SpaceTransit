using SpaceTransit.Ships.Modules;

namespace SpaceTransit.Ships.Driving.Screens
{

    public abstract class ScreenManagerBase<T> : ModuleComponentBase where T : ScreenBase
    {

        public T Screen { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Screen = GetComponent<T>();
        }

        protected virtual void OnEnable() => Screen.SetVisibility(true);

        protected virtual void OnDisable() => Screen.SetVisibility(false);

        protected override void OnInitialized() => Screen.Initialize();

    }

}

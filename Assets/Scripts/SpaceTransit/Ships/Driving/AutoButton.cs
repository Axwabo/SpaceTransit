namespace SpaceTransit.Ships.Driving
{

    public sealed class AutoButton : ToggleButtonBase
    {

        private AutomaticDriver _auto;

        private ManualSpeedControl _manual;

        protected override void OnInitialized()
        {
            if (!Controller.TryGetComponent(out _auto))
            {
                gameObject.SetActive(false);
                return;
            }

            if (!Controller.TryGetComponent(out _manual))
                _manual = Controller.gameObject.AddComponent<ManualSpeedControl>();
            _manual.enabled = !_auto.enabled;
            if (_auto.enabled)
                Press();
        }

        public override void OnInteracted()
        {
            if (Controller.State != ShipState.Docked)
                return;
            _auto.enabled = !_auto.enabled;
            _manual.enabled = !_auto.enabled;
            if (_auto.enabled)
                Press();
            else
                Release();
        }

        private void Update()
        {
            if (!_auto.enabled)
                _manual.enabled = Assembly.IsPlayerMounted;
        }

    }

}

using SpaceTransit.Cosmos;
using SpaceTransit.Routes;
using UnityEngine;

namespace SpaceTransit.Ships.Driving.Screens
{

    [RequireComponent(typeof(ExitList))]
    public sealed class ExitListManager : ScreenManagerBase<ExitList>
    {

        private bool _loaded;

        protected override void OnEnable()
        {
            if (!Parent)
                return;
            base.OnEnable();
            _loaded = true;
            OnStateChanged();
        }

        private void Update()
        {
            if (_loaded)
                return;
            _loaded = true;
            UpdateList();
        }

        public override void OnStateChanged()
        {
            if (State == ShipState.Docked)
                UpdateList();
            else
                Screen.Clear();
        }

        private void UpdateList()
        {
            if (Assembly.FrontModule.Thruster.Tube is not Dock dock)
                return;
            foreach (var exit in dock.FrontExits)
                Screen.Source.Add(new ExitPicker(exit));
            foreach (var exit in dock.BackExits)
                Screen.Source.Add(new ExitPicker(exit));
            Screen.Refresh();
        }

        public void Mark(Exit exit)
        {
            if (!isActiveAndEnabled || Screen.Source.Count == 0)
                return;
            foreach (var picker in Screen.Source)
            {
                if (picker.Exit != exit)
                    continue;
                Assembly.Lock(picker);
                break;
            }
        }

        public bool TryGetPicked(out Exit exit)
        {
            if (!isActiveAndEnabled)
            {
                exit = null;
                return false;
            }

            foreach (var picker in Screen.Source)
            {
                if (!picker.Success)
                    continue;
                exit = picker.Exit;
                return true;
            }

            if (Screen.Selected != -1)
            {
                exit = Screen.Source[Screen.Selected].Exit;
                return true;
            }

            exit = null;
            return false;
        }

    }

}

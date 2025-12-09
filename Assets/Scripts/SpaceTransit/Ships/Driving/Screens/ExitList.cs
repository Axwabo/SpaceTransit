using System.Collections.Generic;
using SpaceTransit.Cosmos;
using SpaceTransit.Routes;

namespace SpaceTransit.Ships.Driving.Screens
{

    public sealed class ExitList : ListBase<Exit, PickablePicker>
    {

        private bool _loaded;

        private void OnEnable()
        {
            if (didStart)
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
            Source.Clear();
            Clear();
            if (State == ShipState.Docked)
                UpdateList();
        }

        private void UpdateList()
        {
            if (Assembly.FrontModule.Thruster.Tube is not Dock dock)
                return;
            Source.AddRange(dock.FrontExits);
            Source.AddRange(dock.BackExits);
        }

        protected override List<Exit> Source { get; } = new();

        protected override bool Select(Exit item, PickablePicker picker)
        {
            if (HasPicked || State != ShipState.Docked)
                return false;
            picker.Selected = true;
            Controller.MarkReady();
            return State == ShipState.WaitingForDeparture;
        }

        protected override string GetContent(Exit item) => item.ConnectedStation.Name;

        public void Mark(Exit exit)
        {
            if (!isActiveAndEnabled || Pickers.Count == 0)
                return;
            var index = Source.IndexOf(exit);
            if (index != -1)
                Pickers[index].Pick(true);
        }

        public bool TryGetPicked(out Exit exit)
        {
            for (var i = 0; i < Pickers.Count; i++)
            {
                if (!Pickers[i].Picked)
                    continue;
                exit = Source[i];
                return true;
            }

            for (var i = 0; i < Pickers.Count; i++)
            {
                if (!Pickers[i].Selected)
                    continue;
                exit = Source[i];
                return true;
            }

            exit = null;
            return false;
        }

    }

}

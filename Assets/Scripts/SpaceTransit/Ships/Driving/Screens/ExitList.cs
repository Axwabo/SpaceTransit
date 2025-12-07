using System.Collections.Generic;
using SpaceTransit.Cosmos;
using SpaceTransit.Routes;

namespace SpaceTransit.Ships.Driving.Screens
{

    public sealed class ExitList : ListBase<Exit, PickablePicker>
    {

        private readonly List<Exit> _exits = new();

        private void OnEnable()
        {
            if (didStart)
                OnStateChanged();
        }

        public override void OnStateChanged()
        {
            _exits.Clear();
            Clear();
            if (State == ShipState.Docked)
                UpdateList();
        }

        protected override void OnInitialized() => UpdateList();

        private void UpdateList()
        {
            if (!Assembly.FrontModule.Thruster.Tube.TryGetComponent(out Dock dock))
                return;
            _exits.AddRange(dock.FrontExits);
            _exits.AddRange(dock.BackExits);
        }

        protected override IReadOnlyList<Exit> Source => _exits;

        protected override bool Select(Exit item, PickablePicker picker)
        {
            if (HasPicked)
                return false;
            var locked = item.Lock(Assembly);
            picker.Pick(locked);
            return locked;
        }

        protected override string GetContent(int index, Exit item) => item.ConnectedStation.Name;

    }

}

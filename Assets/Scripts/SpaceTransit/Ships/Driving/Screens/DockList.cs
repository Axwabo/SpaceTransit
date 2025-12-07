using System.Collections.Generic;
using SpaceTransit.Routes;

namespace SpaceTransit.Ships.Driving.Screens
{

    public sealed class DockList : ListBase<Dock, PickablePicker>
    {

        private string _previousStationName;

        public Station TowardsStation { get; private set; }

        public override void OnStateChanged()
        {
            if (State != ShipState.Sailing)
                Clear();
        }

        private void OnEnable()
        {
            if (didStart)
                Update();
        }

        private void Update()
        {
            var tube = Assembly.FrontModule.Thruster.Tube;
            if (tube.TryGetEntryEnsurer(Assembly.Reverse, out var ensurer))
                UpdateStation(ensurer.station.ID);
            else
                Clear();
        }

        private void UpdateStation(StationId id)
        {
            var stationName = id.name;
            if (_previousStationName == stationName)
                return;
            _previousStationName = stationName;
            Clear();
            if (!Station.TryGetLoadedStation(id, out var station))
            {
                TowardsStation = null;
                return;
            }

            TowardsStation = station;
            SetUp();
        }

        protected override IReadOnlyList<Dock> Source => TowardsStation.Docks;

        protected override bool Select(Dock item, PickablePicker picker)
        {
            if (HasPicked)
                return false;
            var entry = Assembly.Reverse ? item.FrontEntry : item.BackEntry;
            var locked = entry && entry.Lock(Assembly);
            picker.Pick(locked);
            return locked;
        }

        protected override string GetContent(int index, Dock item) => $"{index + 1}";

    }

}

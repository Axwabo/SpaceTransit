using System.Collections.Generic;
using SpaceTransit.Cosmos;
using SpaceTransit.Loader;
using SpaceTransit.Routes;

namespace SpaceTransit.Ships.Driving.Screens
{

    public sealed class EntryList : ListBase<Entry, PickablePicker>
    {

        private StationId _previousStation;

        private EntryEnsurer _ensurer;

        public override void OnStateChanged()
        {
            if (State != ShipState.Sailing)
                Clear();
            if (State == ShipState.Docked)
                _previousStation = null;
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
                UpdateStation(ensurer);
            else
                Clear();
        }

        private void UpdateStation(EntryEnsurer ensurer)
        {
            if (LoadingProgress.Current != null || ensurer.Entries.Count == 0)
                return;
            var station = ensurer.station;
            if (_previousStation == station)
                return;
            _previousStation = station;
            _ensurer = ensurer;
            _ensurer.Entries.Sort(static (a, b) => a.Dock.Index - b.Dock.Index);
            Clear();
            SetUp();
        }

        protected override List<Entry> Source => _ensurer.Entries;

        public override bool Select(int index)
        {
            for (var i = 0; i < Pickers.Count; i++)
                if (Source[i].Dock.Index == index)
                    return Select(Source[i], Pickers[i]);
            return false;
        }

        protected override bool Select(Entry item, PickablePicker picker)
        {
            if (HasPicked)
                return false;
            var locked = item.Lock(Assembly);
            picker.Pick(locked);
            return locked;
        }

        protected override string GetContent(Entry item) => $"{item.Dock.Index + 1}";

        public bool SelectDock(int dockIndex) => Pickers.Count != 0 && isActiveAndEnabled && Select(dockIndex);

    }

}

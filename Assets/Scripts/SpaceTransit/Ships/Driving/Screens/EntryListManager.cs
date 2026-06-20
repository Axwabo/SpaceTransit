using SpaceTransit.Cosmos;
using SpaceTransit.Loader;
using SpaceTransit.Routes;
using UnityEngine;

namespace SpaceTransit.Ships.Driving.Screens
{

    [RequireComponent(typeof(EntryList))]
    public sealed class EntryListManager : ScreenManagerBase<EntryList>
    {

        private StationId _previousStation;

        public override void OnStateChanged()
        {
            if (State != ShipState.Sailing)
                Screen.Clear();
            if (State == ShipState.Docked)
                _previousStation = null;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (didStart)
                Update();
        }

        private void Update()
        {
            var tube = Assembly.FrontModule.Thruster.Tube;
            if (tube.TryGetEntryEnsurer(Assembly.Reverse, out var ensurer))
                UpdateStation(ensurer);
            else
                Screen.Clear();
        }

        private void UpdateStation(EntryEnsurer ensurer)
        {
            if (LoadingProgress.Current != null || ensurer.Entries.Count == 0)
                return;
            var station = ensurer.station;
            if (_previousStation == station)
                return;
            _previousStation = station;
            Screen.Source.Clear();
            foreach (var entry in ensurer.Entries)
                Screen.Source.Add(new EntryPicker(entry));
            Screen.Source.Sort((a, b) => a.Entry.Dock.Index - b.Entry.Dock.Index);
            Screen.Refresh();
        }

        public bool SelectDock(int dockIndex) => Screen.CanPick && isActiveAndEnabled && Screen.Select(dockIndex);

    }

}

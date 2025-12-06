using System.Collections.Generic;
using SpaceTransit.Routes;
using SpaceTransit.Ships.Driving.Screens;
using UnityEngine;

namespace SpaceTransit.Ships.Driving
{

    public sealed class DockList : ScreenBase
    {

        private readonly List<DockPicker> _list = new();

        [SerializeField]
        private DockPicker prefab;

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

        private void Clear()
        {
            foreach (var picker in _list)
                Destroy(picker.gameObject);
            _list.Clear();
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
            for (var i = 0; i < station.Docks.Count; i++)
            {
                var clone = Instantiate(prefab, Transform);
                clone.Index = i;
                _list.Add(clone);
            }
        }

        public override void Navigate(bool forwards)
        {
            if (_list.Count == 0)
                return;
            var previous = Selected;
            var index = previous + (forwards ? 1 : 0);
            if (index >= _list.Count)
                index = 0;
            else if (index < 0)
                index = _list.Count - 1;
            if (previous != -1 && previous != index)
                _list[previous].Selected = false;
            _list[index].Selected = true;
        }

        public bool Select(int index)
        {
            if (index == -1 || _list.Count == 0)
                return false;
            var dock = TowardsStation.Docks[index];
            var entry = Assembly.Reverse ? dock.FrontEntry : dock.BackEntry;
            var locked = entry && entry.Lock(Assembly);
            _list[index].Pick(locked);
            return locked;
        }

        public override void Confirm() => Select(Selected);

        public int Selected => _list.FindIndex(static e => e.Selected);

    }

}

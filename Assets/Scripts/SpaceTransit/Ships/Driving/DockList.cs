using System.Collections.Generic;
using SpaceTransit.Cosmos;
using SpaceTransit.Routes;
using SpaceTransit.Ships.Modules;
using SpaceTransit.Tubes;
using UnityEngine;

namespace SpaceTransit.Ships.Driving
{

    public sealed class DockList : ModuleComponentBase
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

        private void Update()
        {
            var tube = Assembly.FrontModule.Thruster.Tube;
            if (UpdateStation(tube))
                return;
            if (Assembly.Reverse ? tube.HasPrevious : tube.HasNext)
                UpdateStation(tube.Next(Assembly.Reverse));
            else
                Clear();
        }

        private void Clear()
        {
            foreach (var picker in _list)
                Destroy(picker.gameObject);
            _list.Clear();
        }

        private bool UpdateStation(TubeBase tube)
        {
            if (tube.Safety is not EntryEnsurer {station: var station, Backwards: var backwards} || backwards != Assembly.Reverse)
                return false;
            UpdateStation(station.ID);
            return true;
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

        public void Select(bool forwards)
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

        public int Selected => _list.FindIndex(static e => e.Selected);

    }

}

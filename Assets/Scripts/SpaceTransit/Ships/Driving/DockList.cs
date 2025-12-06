using SpaceTransit.Cosmos;
using SpaceTransit.Routes;
using SpaceTransit.Ships.Modules;
using TMPro;
using UnityEngine;

namespace SpaceTransit.Ships.Driving
{

    public sealed class DockList : ModuleComponentBase
    {

        [SerializeField]
        private TextMeshProUGUI prefab;

        private string _previousStationName;

        public override void OnStateChanged()
        {
            if (State == ShipState.Sailing && Controller.TryGetVaulter(out var controller) && controller.IsInService)
                UpdateStation(controller.Stop.Station);
        }

        private void Update()
        {
            if (Assembly.FrontModule.Thruster.Tube.Safety is EntryEnsurer {station: var station})
                UpdateStation(station.ID);
        }

        private void UpdateStation(StationId id)
        {
            var stationName = id.name;
            if (_previousStationName == stationName)
                return;
            _previousStationName = stationName;
            var count = Transform.childCount;
            for (var i = 0; i < count; i++)
                Destroy(Transform.GetChild(i));
            if (!Station.TryGetLoadedStation(id, out var station))
                return;
            for (var i = 0; i < station.Docks.Count; i++)
                Instantiate(prefab, Transform).text = $"{i + 1}";
        }

    }

}

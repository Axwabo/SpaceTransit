using TMPro;
using UnityEngine;

namespace SpaceTransit.Ships.Modules.Displays
{

    public sealed class SpeedLimitDisplay : ModuleComponentBase
    {

        [SerializeField]
        private TextMeshProUGUI main;

        [SerializeField]
        private TextMeshProUGUI next;

        private bool _everDisplayed;

        private float _previous;

        private float _previousNext;

        private void Update()
        {
            if (_everDisplayed && Assembly.IsStationary())
                return;
            _everDisplayed = true;
            var tube = Assembly.FrontModule.Thruster.Tube;
            var limit = tube.SpeedLimit;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (limit != _previous)
            {
                main.text = Limit(limit);
                _previous = limit;
            }

            if (Assembly.Reverse ? !tube.HasPrevious : !tube.HasNext)
            {
                next.text = "";
                return;
            }

            var nextTube = Assembly.Reverse ? tube.Previous : tube.Next;
            var nextLimit = nextTube.SpeedLimit;
            if (Mathf.Approximately(nextLimit, _previousNext))
                return;
            next.text = Mathf.Approximately(nextLimit, limit) ? "" : $"Next: {Limit(nextLimit)}";
            _previousNext = nextLimit;
        }

        private static string Limit(float limit) => limit == 0 ? "--" : (limit * ShipSpeed.MpsToKmh).ToString("N0");

    }

}

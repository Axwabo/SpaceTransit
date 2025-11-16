using SpaceTransit.Tubes;
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

        private void Update()
        {
            if (_everDisplayed && Assembly.IsStationary())
                return;
            _everDisplayed = true;
            var tube = Assembly.FrontModule.Thruster.Tube;
            main.text = Limit(tube);
            if (Assembly.Reverse ? !tube.HasPrevious : !tube.HasNext)
            {
                next.text = "";
                return;
            }

            var nextTube = Assembly.Reverse ? tube.Previous : tube.Next;
            next.text = Mathf.Approximately(nextTube.SpeedLimit, tube.SpeedLimit) ? "" : $"Next: {Limit(nextTube)}";
        }

        private static string Limit(TubeBase tube) => tube.SpeedLimit == 0 ? "--" : (tube.SpeedLimit * 3.6).ToString("N0");

    }

}

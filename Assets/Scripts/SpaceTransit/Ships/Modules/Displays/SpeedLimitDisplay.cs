using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Modules.Displays
{

    [RequireComponent(typeof(UIDocument))]
    public sealed class SpeedLimitDisplay : ModuleComponentBase
    {

        private bool _everDisplayed;

        private float _previous;

        private float _previousNext;

        private Label _main;

        private Label _next;

        private void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            _main = root.Q<Label>("Limit");
            _next = root.Q<Label>("Next");
        }

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
                _main.text = Limit(limit);
                _previous = limit;
            }

            if (Assembly.Reverse ? !tube.HasPrevious : !tube.HasNext)
            {
                _next.text = "";
                return;
            }

            var nextTube = Assembly.Reverse ? tube.Previous : tube.Next;
            var nextLimit = nextTube.SpeedLimit;
            if (Mathf.Approximately(nextLimit, _previousNext))
                return;
            _next.text = Mathf.Approximately(nextLimit, limit) ? "" : $"Next: {Limit(nextLimit)}";
            _previousNext = nextLimit;
        }

        private static string Limit(float limit) => limit == 0 ? "--" : (limit * ShipSpeed.MpsToKmh).ToString("N0");

    }

}

using System.Collections.Generic;
using SpaceTransit.Vaulter;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Stations
{

    public sealed class StationBoard : MonoBehaviour
    {

        [CreateProperty]
        public string Station { get; set; }

        [CreateProperty]
        public List<StationBoardItem> Departures { get; } = new();

        [CreateProperty]
        public List<StationBoardItem> Arrivals { get; } = new();

        private readonly List<UIDocument> _documents = new();

        private DeparturesArrivals _departuresArrivals;

        private int _previousMinute;

        private int _previousDay;

        private void OnEnable()
        {
            if (didStart)
                Init();
        }

        private void Start()
        {
            _departuresArrivals = GetComponentInParent<DeparturesArrivals>();
            Station = _departuresArrivals.StationId.name;
            GetComponentsInChildren(_documents);
            Init();
        }

        private void Update()
        {
            var minute = Clock.Now.Minutes;
            if (_previousMinute == minute)
                return;
            _previousMinute = minute;
            var day = Clock.Date.Day;
            if (day == _previousDay)
            {
                Departures.RemoveAll(e => e.TimeOfDay < Clock.Now);
                Arrivals.RemoveAll(e => e.TimeOfDay < Clock.Now);
                return;
            }

            _previousDay = day;
            Departures.Clear();
            Arrivals.Clear();
            foreach (var departure in _departuresArrivals.Departures)
                if (departure.Departure.Departure >= Clock.Now)
                    Departures.Add(departure);
            foreach (var arrival in _departuresArrivals.Arrivals)
                if (arrival.Arrival.Arrival >= Clock.Now)
                    Arrivals.Add(arrival);
        }

        private void Init()
        {
            _previousDay = _previousMinute = -1;
            foreach (var document in _documents)
            {
                var root = document.rootVisualElement;
                root.dataSource = this;
                foreach (var view in root.Query<ListView>().Build())
                    view.bindItem = (element, i) =>
                    {
                        var item = (StationBoardItem) view.itemsSource[i];
                        element.Q<Label>("Type").text = item.Type;
                        element.Q<Label>("Station").text = item.Station;
                        element.Q<Label>("Time").text = item.Time;
                        element.Q<Label>("DockIndex").text = item.Dock;
                    };
            }
        }

    }

}

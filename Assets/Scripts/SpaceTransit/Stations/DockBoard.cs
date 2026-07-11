using System;
using System.Collections.Generic;
using SpaceTransit.Routes;
using SpaceTransit.Vaulter;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Stations
{

    public sealed class DockBoard : MonoBehaviour
    {

        [SerializeField]
        private Dock dock;

        private readonly List<StopEntry> _entries = new();

        private readonly List<UIDocument> _documents = new();

        private float _delay;
        private DeparturesArrivals _departuresArrivals;

        [CreateProperty]
        public string Dock { get; set; }

        [CreateProperty]
        public string Action { get; set; }

        [CreateProperty]
        public string ShortType { get; private set; }

        [CreateProperty]
        public string LongType { get; private set; }

        [CreateProperty]
        public Color Color { get; private set; }

        [CreateProperty]
        public string Station { get; set; }

        [CreateProperty]
        public string Time { get; set; }

        private void OnEnable()
        {
            if (didStart)
                Init();
        }

        private void Start()
        {
            GetComponentsInChildren(_documents);
            Init();
            _departuresArrivals = GetComponentInParent<DeparturesArrivals>();
        }

        private void Init()
        {
            foreach (var document in _documents)
                document.rootVisualElement.dataSource = this;
        }

        private void Update()
        {
            if ((_delay -= Clock.Delta) > 0)
                return;
            _delay = 10;
            var now = Clock.Now;
            foreach (var entry in _entries)
            {
                if (entry.Time < now)
                    continue;
                LongType = entry.Route.Type switch
                {
                    ServiceType.Passenger => "passenger",
                    ServiceType.Fast => "fast",
                    ServiceType.InterHub => "IH",
                    _ => throw new InvalidOperationException()
                };
                Time = entry.Time.ToString();
                (ShortType, Color) = entry.Route.GetAbbreviation();
                (Station, Action) = entry switch
                {
                    ArrivalEntry arrivalEntry => (arrivalEntry.Route.Origin.Station.name, "Arrives"),
                    DepartureEntry departureEntry => (departureEntry.Route.Destination.Station.name, "Departs"),
                    _ => throw new InvalidOperationException()
                };

                return;
            }

            LongType = ShortType = Time = Station = Action = "";
        }

        private void LateUpdate()
        {
            if (!string.IsNullOrEmpty(Dock))
                return;
            Dock = (dock.Index + 1).ToString();
            foreach (var entry in _departuresArrivals.Departures)
                if (entry.Departure.DockIndex == dock.Index)
                    _entries.Add(entry);
            foreach (var entry in _departuresArrivals.Arrivals)
                if (entry.Index == -1 && entry.Arrival.DockIndex == dock.Index)
                    _entries.Add(entry);
        }

    }

}

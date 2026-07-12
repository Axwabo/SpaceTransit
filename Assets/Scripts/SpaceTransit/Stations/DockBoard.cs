using System;
using System.Collections.Generic;
using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
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

        [SerializeField]
        private MeshRenderer[] renderers;

        [SerializeField]
        private Material arriving;

        [SerializeField]
        private Material departing;

        private readonly List<StopEntry> _entries = new();

        private readonly List<UIDocument> _documents = new();

        private StopEntry _previous;

        private float _delay;

        private DeparturesArrivals _departuresArrivals;

        private bool _arriving;

        [CreateProperty]
        public string Dock { get; set; }

        [CreateProperty]
        public string Action { get; set; }

        [CreateProperty]
        public string ShortType { get; private set; }

        [CreateProperty]
        public string LongType { get; private set; }

        [CreateProperty]
        public string FullType { get; private set; }

        [CreateProperty]
        public Color Color { get; private set; }

        [CreateProperty]
        public string Station { get; private set; }

        [CreateProperty]
        public string Time { get; private set; }

        [CreateProperty]
        public string Hours { get; private set; }

        [CreateProperty]
        public string Minutes { get; private set; }

        [CreateProperty]
        // ReSharper disable once CollectionNeverQueried.Global
        public List<StopItem> Stops { get; } = new();

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
            {
                var root = document.rootVisualElement;
                root.dataSource = this;
            }
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
                if (!ReferenceEquals(entry, _previous))
                    Display(entry);
                return;
            }

            if (_previous is null)
                return;
            _previous = null;
            FullType = LongType = ShortType = Time = Station = Action = "";
            Stops.Clear();
            RefreshLists();
        }

        private void Display(StopEntry entry)
        {
            _previous = entry;
            FullType = entry.Route.Type.ToStringFast();
            LongType = entry.Route.Type switch
            {
                ServiceType.Passenger => "passenger",
                ServiceType.Fast => "fast",
                ServiceType.InterHub => "IH",
                _ => throw new InvalidOperationException()
            };
            Time = entry.Time.ToString();
            (ShortType, Color) = entry.Route.GetAbbreviation();
            (Station, Action, _arriving) = entry switch
            {
                ArrivalEntry arrivalEntry => (arrivalEntry.Route.Origin.Station.name, "arr.", true),
                DepartureEntry departureEntry => (departureEntry.Route.Destination.Station.name, "dep.", false),
                _ => throw new InvalidOperationException()
            };
            Hours = entry.Time.Value.Hours.ToString();
            Minutes = entry.Time.Value.Minutes.ToString("00");
            Stops.Clear();
            if (entry is ArrivalEntry)
            {
                RefreshLists();
                return;
            }

            var stops = entry.Route.IntermediateStops;
            for (var i = entry.Index + 1; i < stops.Length; i++)
                Stops.Add(new StopItem(stops[i].Station.name, stops[i].Arrival.ToString()));
            Stops.Add(new StopItem(entry.Route.Destination.Station.name, entry.Route.Destination.Arrival.ToString()));
            RefreshLists();
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
                if (entry.Index == ITarget.Destination && entry.Arrival.DockIndex == dock.Index)
                    _entries.Add(entry);
            _entries.Sort((a, b) => a.Time.Value.CompareTo(b.Time.Value));
        }

        private void RefreshLists()
        {
            foreach (var document in _documents)
                document.rootVisualElement.Q<ListView>()?.RefreshItems();
            foreach (var meshRenderer in renderers)
                meshRenderer.sharedMaterial = _arriving ? arriving : departing;
        }

    }

}

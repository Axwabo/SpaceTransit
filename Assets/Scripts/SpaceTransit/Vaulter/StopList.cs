using System.Collections.Generic;
using SpaceTransit.Routes.Stops;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Vaulter
{

    public sealed class StopList : VaulterComponentBase
    {

        [SerializeField]
        private VisualTreeAsset prefab;

        private readonly List<Stop> _stops = new();

        private ListView _list;

        private void OnEnable()
        {
            if (Parent)
                OnRouteChanged();
        }

        protected override void OnInitialized()
        {
            _list = this.RootVisual().Q<ListView>("Stops");
            _list.itemsSource = _stops;
            _list.makeItem = () =>
            {
                var element = new VisualElement();
                prefab.CloneTree(element);
                return element;
            };
            _list.bindItem = (element, i) =>
            {
                var stop = _stops[i];
                element.Q<Label>("Station").text = stop.Station.name;
                element.Q<Label>("Arrival").text = (stop as IArrival)?.Arrival.ToString() ?? "";
                element.Q<Label>("Departure").text = (stop as IDeparture)?.Departure.ToString() ?? "";
                element.Q<Label>("DockIndex").text = (stop.DockIndex + 1).ToString();
            };
        }

        public override void OnRouteChanged()
        {
            RefreshStops();
            _list.RefreshItems();
        }

        public override void OnStopChanged()
        {
            if (_stops.Count == 0)
                return;
            _stops.RemoveAt(0);
            _list.RefreshItems();
        }

        private void RefreshStops()
        {
            if (!IsInService)
            {
                _stops.Clear();
                return;
            }

            _stops.Add(Parent.Stop);
            if (Parent.Stop is Destination)
                return;
            foreach (var stop in Parent.NextIntermediateStops)
                _stops.Add(stop);
            _stops.Add(Parent.Route.Destination);
        }

        public void Navigate(bool forwards)
        {
            // TODO
        }

        public void ResetPosition()
        {
            // TODO
        }

    }

}

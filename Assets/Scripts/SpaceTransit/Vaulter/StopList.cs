using System.Collections.Generic;
using SpaceTransit.Routes.Stops;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Vaulter
{

    public sealed class StopList : VaulterComponentBase
    {

        private readonly List<Stop> _stops = new();

        private ListView _list;

        private ScrollView _scrollView;

        private float? _itemHeight;

        private void OnEnable()
        {
            if (Parent)
                OnRouteChanged();
        }

        protected override void OnInitialized()
        {
            _list = this.RootVisual().Q<ListView>("Stops");
            _list.itemsSource = _stops;
            _list.bindItem = (element, i) =>
            {
                var stop = _stops[i];
                element.Q<Label>("Station").text = stop.Station.name;
                element.Q<Label>("Arrival").text = (stop as IArrival)?.Arrival.ToString() ?? "";
                element.Q<Label>("Departure").text = (stop as IDeparture)?.Departure.ToString() ?? "";
                element.Q<Label>("DockIndex").text = (stop.DockIndex + 1).ToString();
            };
            _scrollView = _list.Q<ScrollView>();
            _scrollView.verticalScrollerVisibility = ScrollerVisibility.Hidden;
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
            ResetPosition();
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
            var container = _scrollView.contentContainer;
            var childCount = container.childCount;
            if (childCount == 0)
                return;
            _itemHeight ??= container.hierarchy.ElementAt(0).layout.height;
            _scrollView.scrollOffset += new Vector2(0, _itemHeight.Value * (forwards ? -1 : 1));
        }

        public void ResetPosition() => _scrollView.scrollOffset = Vector2.zero;

        public void SetVisibility(bool visible) => _list.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;

    }

}

using System;
using System.Collections.Generic;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Vaulter;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Driving.Screens
{

    [RequireComponent(typeof(StopListManager))]
    public sealed class StopList : ListBase<ITarget>
    {

        public static Action<VisualElement, int> CreateBindFunction(List<ITarget> stops) => (element, i) => Bind(stops, element, i);

        private static void Bind(List<ITarget> stops, VisualElement element, int i)
        {
            var stop = stops[i];
            element.Q<Label>("Station").text = stop.Station.name;
            element.Q<Label>("Arrival").text = (stop as IArrival)?.Arrival.ToString() ?? "";
            element.Q<Label>("Departure").text = (stop as IDeparture)?.Departure.ToString() ?? "";
            element.Q<Label>("DockIndex").text = (stop.DockIndex + 1).ToString();
            element.EnableInClassList("passthrough", stop is Passthrough);
        }

        private ScrollView _scrollView;

        protected override void Initialize(VisualElement root)
        {
            base.Initialize(root);
            _scrollView = List.Q<ScrollView>();
        }

        protected override ListView GetListView(VisualElement root) => root.Q<ListView>("Stops");

        protected override void BindItem(VisualElement element, int i) => Bind(Source, element, i);

        public override void Navigate(bool forwards)
        {
            if (_scrollView != null)
                _scrollView.scrollOffset += new Vector2(0, List.fixedItemHeight * (forwards ? -1 : 1));
        }

        public override void Confirm() => ResetPosition();

        public void ResetPosition()
        {
            if (_scrollView != null)
                _scrollView.scrollOffset = Vector2.zero;
        }

    }

}

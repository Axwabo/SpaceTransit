using System;
using System.Collections.Generic;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Vaulter;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Driving.Screens
{

    [RequireComponent(typeof(StopListManager))]
    public sealed class StopList : ListBase<StopListEntry>
    {

        public static Action<VisualElement, int> CreateBindFunction(List<ITarget> stops) => (element, i) => Bind(element, stops[i]);

        private static void Bind(VisualElement element, ITarget target) => Bind(
            element,
            target.Station.name,
            (target as IArrival)?.Arrival.ToString() ?? "",
            (target as IDeparture)?.Departure.ToString() ?? "",
            (target.DockIndex + 1).ToString()
        );

        private static void Bind(VisualElement element, string station, string arrival, string departure, string dockIndex)
        {
            element.Q<Label>("Station").text = station;
            element.Q<Label>("Arrival").text = arrival;
            element.Q<Label>("Departure").text = departure;
            element.Q<Label>("DockIndex").text = dockIndex;
        }

        private ScrollView _scrollView;

        protected override void Initialize(VisualElement root)
        {
            base.Initialize(root);
            _scrollView = List.Q<ScrollView>();
        }

        protected override ListView GetListView(VisualElement root) => root.Q<ListView>("Stops");

        protected override void BindItem(VisualElement element, int i)
        {
            var entry = Source[i];
            element.EnableInClassList("separator", i < Source.Count - 1 && Source[i + 1] is not ExitTowards);
            element.EnableInClassList("passthrough", entry.Target is Passthrough);
            if (entry is not ExitTowards exitTowards)
            {
                Bind(element, entry.Target);
                element.RemoveFromClassList("indent");
                return;
            }

            Bind(element, $"» {exitTowards.Exit.ExitTowards.name}", "", "", "");
            element.AddToClassList("indent");
        }

        public override void Navigate(bool forwards)
        {
            if (_scrollView != null)
                _scrollView.scrollOffset += new Vector2(0, List.fixedItemHeight * (forwards ? 1 : -1));
        }

        public override void Confirm() => ResetPosition();

        public void ResetPosition()
        {
            if (_scrollView != null)
                _scrollView.scrollOffset = Vector2.zero;
        }

        public bool IsAtZeroOffset => _scrollView != null && Mathf.Approximately(0, _scrollView.scrollOffset.y);

    }

}

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

        private static void Bind(VisualElement element, ITarget target)
        {
            element.Q<Label>("Station").text = target.Station.name;
            element.Q<Label>("Arrival").text = (target as IArrival)?.Arrival.ToString() ?? "";
            element.Q<Label>("Departure").text = (target as IDeparture)?.Departure.ToString() ?? "";
            element.Q<Label>("DockIndex").text = (target.DockIndex + 1).ToString();
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
            element.EnableInClassList("separator", i < Source.Count - 1 && Source[i + 1] is not ExitTowards);
            if (Source[i] is not ExitTowards exitTowards)
            {
                Bind(element, Source[i].Target);
                element.RemoveFromClassList("indent");
                return;
            }

            element.Q<Label>("Station").text = $"» {exitTowards.Exit.ExitTowards.name}";
            element.AddToClassList("indent");
        }

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

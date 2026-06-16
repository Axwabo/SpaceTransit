using System.Collections.Generic;
using SpaceTransit.Routes.Stops;
using UnityEngine.UIElements;

namespace SpaceTransit.Vaulter
{

    public sealed class StopList : VaulterComponentBase
    {

        private readonly List<Stop> _stops = new();

        private MultiColumnListView _list;

        private void OnEnable()
        {
            if (Parent)
                OnRouteChanged();
        }

        protected override void OnInitialized()
        {
            _list = this.RootVisual().Q<MultiColumnListView>("Stops");
            _list.itemsSource = _stops;
        }

        public override void OnRouteChanged()
        {
            if (!IsInService)
            {
                _stops.Clear();
                _list.RefreshItems();
                return;
            }

            _stops.Add(Parent.Stop);
            if (Parent.Stop is Destination)
                return;
            foreach (var stop in Parent.NextIntermediateStops)
                _stops.Add(stop);
            _stops.Add(Parent.Route.Destination);
        }

        public override void OnStopChanged()
        {
            if (_stops.Count == 0)
                return;
            _stops.RemoveAt(0);
            _list.RefreshItems();
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

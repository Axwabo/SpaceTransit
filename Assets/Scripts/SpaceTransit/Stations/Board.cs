using System;
using System.Collections.Generic;
using SpaceTransit.Routes;
using SpaceTransit.Vaulter;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Stations
{

    public abstract class Board<TEntry> : MonoBehaviour, ICullingListener
    {

        private readonly List<TEntry> _items = new();

        private DeparturesArrivals _departuresArrivals;

        private ListView _list;

        private int _previousMinute;

        protected abstract string ClassName { get; }

        protected abstract List<TEntry> GetSource(DeparturesArrivals departuresArrivals);

        protected abstract TimeOnly GetTime(TEntry entry);

        protected abstract void Bind(VisualElement element, TEntry entry);

        private void Start()
        {
            _departuresArrivals = GetComponentInParent<DeparturesArrivals>();
            _list = this.RootVisual().Q(className: ClassName).Q<ListView>();
            _list.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;
            _list.itemsSource = _items;
            _list.bindItem = BindItem;
        }

        private void OnEnable() => _previousMinute = -1;

        private void Update()
        {
            var minute = Clock.Now.Minutes;
            if (_previousMinute == minute)
                return;
            _previousMinute = minute;
            var count = _items.Count;
            _items.Clear();
            foreach (var entry in GetSource(_departuresArrivals))
                if (GetTime(entry).Value >= Clock.Now)
                    _items.Add(entry);
            if (_items.Count != count)
                _list.RefreshItems();
        }

        private void BindItem(VisualElement element, int i) => Bind(element, _items[i]);

        protected void Apply(VisualElement element, ServiceType serviceType, string stationName, TimeSpan timeValue, int dockIndex)
        {
            element.Q<Label>("Type").text = serviceType.ToStringFast();
            element.Q<Label>("Station").text = stationName;
            element.Q<Label>("Time").text = timeValue.ToString(TimeOnly.Format);
            element.Q<Label>("DockIndex").text = (dockIndex + 1).ToString();
        }

    }

}

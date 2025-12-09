using System.Collections.Generic;
using SpaceTransit.Vaulter;
using UnityEngine;

namespace SpaceTransit.Stations
{

    public abstract class Board<TEntry, TPrefab> : MonoBehaviour where TPrefab : EntryDisplayBase<TEntry>
    {

        [SerializeField]
        private TPrefab prefab;

        private DeparturesArrivals _departuresArrivals;

        private int _previousMinute;

        private Transform _t;

        private readonly List<TPrefab> _list = new();

        protected abstract List<TEntry> GetSource(DeparturesArrivals departuresArrivals);

        protected abstract TimeOnly GetTime(TEntry entry);

        private void Awake()
        {
            _departuresArrivals = GetComponentInParent<DeparturesArrivals>();
            _t = transform;
        }

        private void OnEnable() => _previousMinute = -1;

        private void Update()
        {
            var minute = Clock.Now.Minutes;
            if (_previousMinute == minute)
                return;
            _previousMinute = minute;
            var i = 0;
            foreach (var entry in GetSource(_departuresArrivals))
            {
                var time = GetTime(entry);
                if (time.Value < Clock.Now)
                    continue;
                var index = i++;
                TPrefab item;
                if (_list.Count > index)
                    (item = _list[index]).gameObject.SetActive(true);
                else
                    _list.Add(item = Instantiate(prefab, _t));
                item.Apply(entry);
            }

            for (var j = i; j < _list.Count; j++)
                _list[i].gameObject.SetActive(false);
        }

    }

}

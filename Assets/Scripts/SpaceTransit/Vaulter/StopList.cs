using System.Collections.Generic;
using SpaceTransit.Menu;
using SpaceTransit.Routes.Stops;
using UnityEngine;

namespace SpaceTransit.Vaulter
{

    public sealed class StopList : VaulterComponentBase
    {

        [SerializeField]
        private StopRow prefab;

        private readonly List<GameObject> _children = new();

        private RectTransform _t;

        protected override void Awake()
        {
            base.Awake();
            _t = (RectTransform) Transform;
        }

        private void OnEnable()
        {
            if (Parent)
                OnRouteChanged();
        }

        public override void OnRouteChanged()
        {
            foreach (var row in _children)
                Destroy(row);
            _children.Clear();
            if (!IsInService)
                return;
            Instantiate(Parent.Stop);
            if (Parent.Stop is Destination)
                return;
            foreach (var stop in Parent.NextIntermediateStops)
                Instantiate(stop);
            Instantiate(Parent.Route.Destination);
        }

        private void Instantiate(Stop stop)
        {
            var clone = Instantiate(prefab, Transform, false);
            clone.Apply(stop);
            _children.Add(clone.gameObject);
        }

        public override void OnStopChanged()
        {
            if (_children.Count == 0)
                return;
            Destroy(_children[0]);
            _children.RemoveAt(0);
        }

        public void Navigate(bool forwards)
        {
            if (_children.Count == 0)
                return;
            var delta = new Vector2(0, ((RectTransform) _children[0].transform).sizeDelta.y);
            if (forwards)
                _t.anchoredPosition -= delta;
            else
                _t.anchoredPosition += delta;
        }

        public void ResetPosition() => _t.anchoredPosition = Vector2.zero;

    }

}

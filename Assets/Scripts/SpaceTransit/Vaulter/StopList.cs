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

    }

}

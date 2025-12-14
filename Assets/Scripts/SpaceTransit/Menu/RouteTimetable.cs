using System.Collections.Generic;
using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using TMPro;
using UnityEngine;

namespace SpaceTransit.Menu
{

    public sealed class RouteTimetable : MonoBehaviour
    {

        [SerializeField]
        private TextMeshProUGUI type;

        [SerializeField]
        private TextMeshProUGUI summary;

        [SerializeField]
        private StopRow prefab;

        private readonly List<StopRow> _children = new();

        public void Apply(RouteDescriptor descriptor)
        {
            foreach (var row in _children)
                Destroy(row.gameObject);
            _children.Clear();
            type.text = $"{descriptor.Type.ToStringFast()} {descriptor.name}";
            summary.text = descriptor.Summary();
            Instantiate(descriptor.Origin);
            foreach (var stop in descriptor.IntermediateStops)
                Instantiate(stop);
            Instantiate(descriptor.Destination);
        }

        private void Instantiate(Stop stop)
        {
            var clone = Instantiate(prefab, transform, false);
            clone.Apply(stop);
            _children.Add(clone);
        }

    }

}

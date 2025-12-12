using SpaceTransit.Routes;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;

namespace SpaceTransit.Menu
{

    public sealed class MapStation : MonoBehaviour
    {

        [SerializeField]
        private TextMeshProUGUI text;

        [SerializeField]
        private PositionConstraint constraint;

        public void Apply(Station station, Transform anchor)
        {
            text.text = station.Name;
            constraint.AddSource(new ConstraintSource
            {
                sourceTransform = anchor,
                weight = 1
            });
        }

    }

}

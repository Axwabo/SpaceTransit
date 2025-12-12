using SpaceTransit.Routes;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;

namespace SpaceTransit.Menu
{

    [RequireComponent(typeof(RectTransform))]
    public sealed class MapStation : MonoBehaviour
    {

        [SerializeField]
        private TextMeshProUGUI text;

        [SerializeField]
        private PositionConstraint constraint;

        private Transform _anchor;

        private RectTransform _this;

        private void Awake() => _this = (RectTransform) transform;

        public void Apply(Station station, Transform anchor)
        {
            text.text = station.Name;
            _anchor = anchor;
            UpdatePosition();
        }

        private void Update() => UpdatePosition();

        private void UpdatePosition() => _this.anchoredPosition = _anchor.localPosition;

    }

}

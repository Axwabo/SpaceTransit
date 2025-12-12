using SpaceTransit.Routes;
using TMPro;
using UnityEngine;

namespace SpaceTransit.Menu
{

    [RequireComponent(typeof(RectTransform))]
    public sealed class MapStation : MonoBehaviour
    {

        [SerializeField]
        private TextMeshProUGUI text;

        private RectTransform _this;

        private void Awake() => _this = (RectTransform) transform;

        public void Apply(Station station, Vector2 position)
        {
            text.text = station.Name;
            _this.anchoredPosition = position;
        }

    }

}

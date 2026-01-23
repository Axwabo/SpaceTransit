using TMPro;
using UnityEngine;

namespace SpaceTransit.Menu
{

    public sealed class KatieSubtitle : MonoBehaviour
    {

        [SerializeField]
        private TextMeshProUGUI main;

        [SerializeField]
        private MonoBehaviour background;

        private double _activateAt;

        private double _removeAt;

        private bool _activated;

        private float _preferredSize;

        public void SetUp(string announcer, string text, double delay, double duration)
        {
            _activateAt = AudioSettings.dspTime + delay;
            _removeAt = _activateAt + duration + 0.5;
            main.text = $"<b>{announcer}:</b> {text}";
        }

        private void Start()
        {
            _preferredSize = main.preferredHeight;
            main.enabled = background.enabled = false;
        }

        private void Update()
        {
            var time = AudioSettings.dspTime;
            if (time >= _removeAt)
                Destroy(gameObject);
            else if (_activated || time < _activateAt)
                return;
            _activated = true;
            main.enabled = background.enabled = true;
            var t = (RectTransform) transform;
            t.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _preferredSize);
            t.SetAsLastSibling();
        }

    }

}

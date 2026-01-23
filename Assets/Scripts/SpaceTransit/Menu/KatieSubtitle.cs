using TMPro;
using UnityEngine;

namespace SpaceTransit.Menu
{

    public sealed class KatieSubtitle : MonoBehaviour
    {

        [SerializeField]
        private TextMeshProUGUI main;

        private double _activateAt;

        private double _removeAt;

        private bool _activated;

        public float PreferredHeight { get; private set; }

        public void SetUp(string announcer, string text, double delay, double duration)
        {
            _activateAt = AudioSettings.dspTime + delay;
            _removeAt = _activateAt + duration + 0.5;
            main.text = $"<b>{announcer}:</b> {text}";
        }

        private void Start()
        {
            PreferredHeight = main.preferredHeight;
            main.gameObject.SetActive(false);
        }

        private void Update()
        {
            var time = AudioSettings.dspTime;
            if (time >= _removeAt)
                Destroy(gameObject);
            else if (_activated || time < _activateAt)
                return;
            _activated = true;
            main.gameObject.SetActive(true);
            KatieSubtitleList.Register(this);
            ((RectTransform) transform).anchoredPosition = new Vector2(0, -KatieSubtitleList.CalculateY(this));
        }

        private void OnDestroy() => KatieSubtitleList.Unregister(this);

    }

}

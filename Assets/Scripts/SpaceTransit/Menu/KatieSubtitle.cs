using TMPro;
using UnityEngine;

namespace SpaceTransit.Menu
{

    public sealed class KatieSubtitle : MonoBehaviour
    {

        [SerializeField]
        private GameObject visibility;

        [SerializeField]
        private TextMeshProUGUI announcerName;

        [SerializeField]
        private TextMeshProUGUI main;

        private double _activateAt;

        private double _removeAt;

        public void SetUp(string announcer, string text, double delay, double duration)
        {
            announcerName.text = $"K.A.T.I.E. ({announcer})";
            main.text = text;
            var time = AudioSettings.dspTime;
            _activateAt = time + delay;
            _removeAt = _activateAt + duration;
        }

        private void Update()
        {
            var time = AudioSettings.dspTime;
            if (time >= _activateAt)
                visibility.SetActive(true);
            else if (time >= _removeAt)
                Destroy(gameObject);
        }

    }

}

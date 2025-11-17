using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceTransit.Menu
{

    public sealed class TimeScaleInput : MonoBehaviour
    {

        [SerializeField]
        private TextMeshProUGUI text;

        [SerializeField]
        private Slider slider;

        public void UpdateValue() => Time.timeScale = slider.value;

        private void Update() => text.text = Time.timeScale.ToString("N");

    }

}

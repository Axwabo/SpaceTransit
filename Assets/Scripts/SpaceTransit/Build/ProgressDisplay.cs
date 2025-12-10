using TMPro;
using UnityEngine;

namespace SpaceTransit.Build
{

    public sealed class ProgressDisplay : MonoBehaviour
    {

        [SerializeField]
        private TextMeshProUGUI text;

        [SerializeField]
        private RectTransform rect;

        private void Update()
        {
            text.text = $"Loading... {GradualTubeLoader.Progress:P0}";
            rect.localScale = new Vector3(GradualTubeLoader.Progress, 1);
        }

    }

}

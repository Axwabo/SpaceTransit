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
            var total = 0;
            var loaded = 0;
            foreach (var loader in GradualTubeLoader.Instances)
            {
                total += loader.Load.Length;
                loaded += loader.Index;
            }

            var progress = total / (float) loaded;
            text.text = progress.ToString("P0");
            rect.localScale = new Vector3(progress, 1);
        }

    }

}

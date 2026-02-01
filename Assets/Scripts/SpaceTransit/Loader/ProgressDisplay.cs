using SpaceTransit.Build;
using TMPro;
using UnityEngine;

namespace SpaceTransit.Loader
{

    public sealed class ProgressDisplay : MonoBehaviour
    {

        [SerializeField]
        private TextMeshProUGUI text;

        [SerializeField]
        private RectTransform rect;

        [SerializeField]
        private GameObject menu;

        private bool _any;

        private void Update()
        {
            if (!_any && GradualTubeLoader.Instances.Count == 0)
                return;
            _any = true;
            var total = 0;
            var loaded = 0;
            foreach (var loader in GradualTubeLoader.Instances)
            {
                total += loader.Load.Length;
                loaded += loader.Index;
            }

            if (total == 0)
                return;

            var progress = total / (float) loaded;
            text.text = progress.ToString("P0");
            rect.localScale = new Vector3(progress, 1);

            if (progress < 1)
                return;
            Destroy(gameObject);
            menu.SetActive(true);
        }

    }

}

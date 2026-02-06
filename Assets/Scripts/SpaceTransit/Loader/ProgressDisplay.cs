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

        private void Update()
        {
            if (LoadingProgress.Current == null)
                return;
            var progress = (float) LoadingProgress.Current.Completed / LoadingProgress.Current.Total;
            text.text = progress.ToString("P0");
            rect.localScale = new Vector3(progress, 1);
            if (progress >= 1)
                LoadingProgress.Current = null;
        }

    }

}

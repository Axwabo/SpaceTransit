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
        private GameObject activate;

        private bool _shouldActivate;

        private void Awake() => _shouldActivate = activate;

        private void Update()
        {
            if (LoadingProgress.Current == null)
            {
                if (_shouldActivate)
                    activate.SetActive(false);
                return;
            }

            if (_shouldActivate)
                activate.SetActive(true);
            var progress = (float) LoadingProgress.Current.Completed / LoadingProgress.Current.Total;
            text.text = progress.ToString("P0");
            rect.localScale = new Vector3(progress, 1);
            if (progress >= 1)
                LoadingProgress.Current = null;
        }

    }

}

using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Loader
{

    public sealed class ProgressDisplay : MonoBehaviour
    {

        private VisualElement _container;

        private Label _percentage;

        private VisualElement _bar;

        private void Start()
        {
            var root = this.RootVisual();
            _container = root.Q<VisualElement>("Loading");
            _percentage = root.Q<Label>("Percentage");
            _bar = root.Q<VisualElement>("Progress");
        }

        private void Update()
        {
            if (LoadingProgress.Current == null || Time.timeSinceLevelLoadAsDouble < Clock.OffsetSeconds + 1)
            {
                _container?.SetVisibility(false);
                return;
            }

            _container?.SetVisibility(true);
            var progress = (float) LoadingProgress.Current.Completed / LoadingProgress.Current.Total;
            _percentage.text = progress.ToString("P0");
            _bar.style.width = Length.Percent(progress * 100);
            if (progress >= 1)
                LoadingProgress.Current = null;
        }

    }

}

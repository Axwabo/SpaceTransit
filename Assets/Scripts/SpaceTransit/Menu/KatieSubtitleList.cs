using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Menu
{

    public sealed class KatieSubtitleList : MonoBehaviour
    {

        private static KatieSubtitleList _current;

        private readonly List<KatieSubtitle> _subtitles = new();

        private ListView _list;

        private void Awake() => _current = this;

        private void Start()
        {
            _list = this.RootVisual().Q<ListView>("Subtitles");
            _list.itemsSource = _subtitles;
            _list.bindItem = (element, i) =>
            {
                var label = element as Label ?? element.Q<Label>();
                label.text = _subtitles[i].Text;
                label.SetVisibility(_subtitles[i].Activated);
            };
        }

        private void Update()
        {
            var refresh = false;
            for (var i = _subtitles.Count - 1; i >= 0; i--)
            {
                var subtitle = _subtitles[i];
                var wasActivated = subtitle.Activated;
                var isAlive = subtitle.Update();
                if (isAlive)
                {
                    refresh |= wasActivated != subtitle.Activated;
                    continue;
                }

                _subtitles.RemoveAt(i);
                refresh = true;
            }

            if (!refresh)
                return;
            _subtitles.Sort((a, b) => a.ActivateAt.CompareTo(b.ActivateAt));
            _list.RefreshItems();
        }

        private void Add(KatieSubtitle subtitle)
        {
            _subtitles.Add(subtitle);
            _list.RefreshItems();
        }

        public static void Add(string announcer, string text, double delay, double duration) => _current.Add(new KatieSubtitle(announcer, text, delay, duration));

    }

}

using System.Collections.Generic;
using UnityEngine;

namespace SpaceTransit.Menu
{

    public sealed class KatieSubtitles : MonoBehaviour
    {

        private readonly List<Subtitle> _subtitles = new();

        private readonly List<Subtitle> _lastDisplayed = new();

        private readonly List<Subtitle> _toDisplay = new();

        private bool _dirty;

        public void Add(string annoucer, string text, double delay, double duration)
        {
            _dirty = true;
            _subtitles.Add(new Subtitle {Announcer = annoucer, Text = text, RemainingDelay = delay, RemainingTime = duration});
        }

        private void Update()
        {
            _toDisplay.Clear();
            for (var i = _subtitles.Count - 1; i >= 0; i--)
            {
                var subtitle = _subtitles[i];
                if (subtitle.)
            }
        }

        private sealed class Subtitle
        {

            public string Announcer;

            public string Text;

            public double RemainingDelay;

            public double RemainingTime;

        }

    }

}

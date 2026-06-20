using UnityEngine;

namespace SpaceTransit.Menu
{

    public sealed class KatieSubtitle
    {

        private readonly double _removeAt;

        public double ActivateAt { get; }

        public bool Activated { get; private set; }

        public string Text { get; }

        public KatieSubtitle(string announcer, string text, double delay, double duration)
        {
            ActivateAt = AudioSettings.dspTime + delay;
            _removeAt = ActivateAt + duration + 0.5;
            Text = $"<b>{announcer}:</b> {text}";
        }

        public bool Update()
        {
            var time = AudioSettings.dspTime;
            if (time >= _removeAt)
                return false;
            if (Activated || time < ActivateAt)
                return true;
            Activated = true;
            return true;
        }

    }

}

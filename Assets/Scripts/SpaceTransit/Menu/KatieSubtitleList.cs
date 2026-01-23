using System.Collections.Generic;
using UnityEngine;

namespace SpaceTransit.Menu
{

    public sealed class KatieSubtitleList : MonoBehaviour
    {

        private static KatieSubtitleList _current;

        [SerializeField]
        private KatieSubtitle prefab;

        private Transform _t;

        private readonly List<KatieSubtitle> _instances = new();

        private void Awake()
        {
            _t = transform;
            _current = this;
        }

        public static void Add(string announcer, string text, double delay, double duration)
        {
            var instance = Instantiate(_current.prefab, _current._t);
            instance.SetUp(announcer, text, delay, duration);
            _current._instances.Add(instance);
        }

        public static void Unregister(KatieSubtitle subtitle) => _current._instances.Remove(subtitle);

        public static float CalculateY(KatieSubtitle subtitle)
        {
            var y = 0f;
            foreach (var instance in _current._instances)
            {
                if (instance == subtitle)
                    break;
                y += subtitle.PreferredHeight + 5;
            }

            return y;
        }

    }

}

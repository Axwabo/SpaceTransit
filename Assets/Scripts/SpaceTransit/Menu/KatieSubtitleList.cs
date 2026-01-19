using UnityEngine;

namespace SpaceTransit.Menu
{

    public sealed class KatieSubtitleList : MonoBehaviour
    {

        private static KatieSubtitleList _current;

        [SerializeField]
        private KatieSubtitle prefab;

        private Transform _t;

        private void Awake()
        {
            _t = transform;
            _current = this;
        }

        public static void Add(string announcer, string text, double delay, double duration)
            => Instantiate(_current.prefab, _current._t).SetUp(announcer, text, delay, duration);

    }

}

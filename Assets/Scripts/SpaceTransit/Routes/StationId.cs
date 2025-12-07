using System.Collections.Generic;
using UnityEngine;

namespace SpaceTransit.Routes
{

    [CreateAssetMenu(fileName = "Station", menuName = "SpaceTransit/Station ID", order = 0)]
    public sealed class StationId : ScriptableObject
    {

        private string _name;

        public string Name => _name ??= name;

        [field: SerializeField]
        public AudioClip Announcement { get; set; }

        [SerializeField]
        private int[] lines;

        public IReadOnlyList<int> Lines => lines;

    }

}

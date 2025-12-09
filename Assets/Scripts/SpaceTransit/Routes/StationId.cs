using System;
using UnityEngine;

namespace SpaceTransit.Routes
{

    [CreateAssetMenu(fileName = "Station", menuName = "SpaceTransit/Station ID", order = 0)]
    public sealed class StationId : ScriptableObject
    {

        [field: SerializeField]
        public AudioClip Announcement { get; set; }

        [SerializeField]
        private int[] lines;

        public ReadOnlySpan<int> Lines => lines;

    }

}

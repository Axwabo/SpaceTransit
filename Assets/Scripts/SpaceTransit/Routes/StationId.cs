using System;
using System.Linq;
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

        [SerializeField]
        private string[] lineNames;

        public ReadOnlySpan<string> Lines => lineNames;

        private void OnValidate()
        {
            if (lines is {Length: not 0})
                lineNames = lines.Select(e => e.ToString()).ToArray();
        }

    }

}

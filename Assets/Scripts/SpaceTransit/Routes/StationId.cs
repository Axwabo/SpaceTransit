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
        [HideInInspector]
        private int[] lines;

        [SerializeField]
        [InspectorName("Lines")]
        private string[] lineNames;

        [SerializeField]
        [HideInInspector]
        public Vector3 position;

        public ReadOnlySpan<string> Lines => lineNames;

        private void OnValidate()
        {
            if (lines is not {Length: not 0})
                return;
            lineNames = lines.Select(e => e.ToString()).ToArray();
            lines = null;
        }

    }

}

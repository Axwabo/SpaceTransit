using System.Collections.Generic;
using UnityEngine;

namespace SpaceTransit.Routes
{

    [CreateAssetMenu(fileName = "Station", menuName = "SpaceTransit/Station ID", order = 0)]
    public sealed class StationId : ScriptableObject
    {

        [SerializeField]
        private int[] lines;

        public IReadOnlyList<int> Lines => lines;

    }

}

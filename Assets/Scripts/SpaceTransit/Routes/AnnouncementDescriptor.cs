using System;
using UnityEngine;

namespace SpaceTransit.Routes
{

    [Serializable]
    public sealed class AnnouncementDescriptor
    {

        [SerializeField]
        private StationId[] via;

        [field: SerializeField]
        public int MinViaDistance { get; private set; }

        [SerializeField]
        private StopSubsetRule[] rules;

        public ReadOnlySpan<StationId> Via => via;

        public ReadOnlySpan<StopSubsetRule> Rules => rules;

    }

}

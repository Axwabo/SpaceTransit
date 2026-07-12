using System;
using UnityEngine;

namespace SpaceTransit.Routes
{

    [Serializable]
    public sealed class AnnouncementDescriptor
    {

        [SerializeField]
        private StationId[] via;

        [SerializeField]
        private StopSubsetRule[] rules;

        public ReadOnlySpan<StationId> Via => via;

        public ReadOnlySpan<StopSubsetRule> Rules => rules;

    }

}

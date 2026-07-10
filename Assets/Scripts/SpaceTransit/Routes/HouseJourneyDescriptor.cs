using System;
using SpaceTransit.Routes.Stops;
using UnityEngine;

namespace SpaceTransit.Routes
{

    [CreateAssetMenu(fileName = "Ship House Journey", menuName = "SpaceTransit/House Journey")]
    public sealed class HouseJourneyDescriptor : JourneyDescriptorBase
    {

        [SerializeField]
        private HouseOrigin origin;

        [SerializeField]
        private HouseDestination destination;

        [SerializeField]
        private Passthrough[] passthrough;

        public override IOrigin Beginning => origin;

        public override IDestination End => destination;

        public override ReadOnlySpan<Passthrough> Passthrough => passthrough;

    }

}

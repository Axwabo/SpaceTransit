using SpaceTransit.Ships;

namespace SpaceTransit.Cosmos
{

    public sealed class TwoSegmentsSafety : SafetyEnsurer
    {

        public override bool CanProceed(ShipAssembly assembly)
        {
            var tube = Tube;
            if (assembly.Reverse ? !tube.HasPrevious : !tube.HasNext)
                return true;
            var next = tube.Next(assembly.Reverse);
            if (!next.Safety.IsFreeFor(assembly))
                return false;
            if (assembly.Reverse ? !next.HasPrevious : !next.HasNext)
                return true;
            return next.Next(assembly.Reverse).Safety.IsFreeFor(assembly);
        }

    }

}

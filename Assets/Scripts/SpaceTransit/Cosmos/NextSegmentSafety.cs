using SpaceTransit.Ships;

namespace SpaceTransit.Cosmos
{

    public sealed class NextSegmentSafety : SafetyEnsurer
    {

        public override bool CanProceed(ShipAssembly assembly)
        {
            if (assembly.Reverse ? !Tube.HasPrevious : !Tube.HasNext)
                return true;
            var next = Tube.Next(assembly.Reverse);
            return !next || next.Safety.IsFreeFor(assembly);
        }

    }

}

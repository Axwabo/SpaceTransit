using SpaceTransit.Ships;

namespace SpaceTransit.Cosmos
{

    public sealed class NextSegmentSafety : SafetyEnsurer
    {

        public override bool CanProceed(ShipAssembly assembly)
            => (assembly.Reverse ? !Tube.HasPrevious : !Tube.HasNext)
               || Tube.Next(assembly.Reverse).Safety.IsFreeFor(assembly);

    }

}

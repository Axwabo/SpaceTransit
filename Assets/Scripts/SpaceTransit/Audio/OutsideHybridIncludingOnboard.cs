using SpaceTransit.Movement;

namespace SpaceTransit.Audio
{

    public sealed class OutsideHybridIncludingOnboard : ShipAudioBase
    {

        private void Update() => Transform.position = IsPlayerMounted
            ? MovementController.Current.LastPosition
            : Assembly.ClosestPoint(MovementController.Current.LastPosition);

    }

}

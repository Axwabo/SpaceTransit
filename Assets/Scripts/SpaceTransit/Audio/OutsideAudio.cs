using SpaceTransit.Movement;

namespace SpaceTransit.Audio
{

    public sealed class OutsideAudio : ShipAudioBase
    {

        private void Update()
        {
            if (IsPlayerMounted)
            {
                Mute = true;
                return;
            }

            Transform.position = Assembly.ClosestPoint(MovementController.Current.Position);
            Mute = false;
        }

    }

}

namespace SpaceTransit.Audio
{

    public sealed class OnboardAudio : ShipAudioBase
    {

        private void FixedUpdate() => Mute = !IsPlayerMounted;

    }

}

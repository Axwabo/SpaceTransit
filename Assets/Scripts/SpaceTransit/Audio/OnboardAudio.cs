namespace SpaceTransit.Audio
{

    public sealed class OnboardAudio : ShipAudioBase
    {

        private void Update() => Mute = !IsPlayerMounted;

    }

}

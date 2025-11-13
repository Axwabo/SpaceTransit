namespace SpaceTransit.Audio
{

    public sealed class OnboardAudio : ShipAudioBase
    {

        private void Update() => Source.mute = !IsPlayerMounted;

    }

}

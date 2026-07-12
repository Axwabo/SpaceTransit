namespace SpaceTransit.Audio
{

    public interface IShipAudioComponent
    {

        bool Mute { get; }

        void UpdateMuteStatus();

    }

}

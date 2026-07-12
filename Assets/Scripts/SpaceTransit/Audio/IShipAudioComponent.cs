using UnityEngine;

namespace SpaceTransit.Audio
{

    public interface IShipAudioComponent
    {

        AudioSource Source { get; }

        bool Mute { get; }

    }

}

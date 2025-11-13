using SpaceTransit.Movement;
using UnityEngine;

namespace SpaceTransit.Audio
{

    [RequireComponent(typeof(AudioSource))]
    public sealed class OutsideAudio : ShipAudioBase
    {

        private void Update()
        {
            if (IsPlayerMounted)
            {
                Source.mute = true;
                return;
            }

            Transform.position = Assembly.ClosestPoint(MovementController.Current.Position);
            Source.mute = false;
        }

    }

}

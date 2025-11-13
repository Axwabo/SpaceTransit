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
                Mute = true;
                return;
            }

            Transform.position = Assembly.ClosestPoint(MovementController.Current.Position);
            Mute = false;
        }

    }

}

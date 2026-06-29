using System.Threading;
using SpaceTransit.Movement;
using UnityEngine;

namespace SpaceTransit.Ships
{

    [CreateAssetMenu(fileName = "Restart Sequence", menuName = "SpaceTransit/Restart Sequence")]
    public sealed class RestartSequence : ScriptableObject
    {

        [SerializeField]
        private AudioClip start;

        [SerializeField]
        private AudioClip beep;

        public async Awaitable Execute(ShipController controller, CancellationToken token)
        {
            if (controller.TryGetVaulter(out var vaulter))
                vaulter.Announcer.AnnounceRestarting();
            while (!controller.ModulesReadyForDeparture)
                await Awaitable.NextFrameAsync(token);
            await Awaitable.WaitForSecondsAsync(10, token);
            // TODO
            AudioSource.PlayClipAtPoint(start, MovementController.Current.Position);
            await Awaitable.WaitForSecondsAsync(5, token);
            FlashAlarms(controller);
            await Awaitable.WaitForSecondsAsync(10, token);
        }

        private void FlashAlarms(ShipController controller)
        {
            foreach (var module in controller.Assembly.Modules)
            foreach (var door in module.Doors)
                door.Alarm.Flash(beep);
        }

    }

}

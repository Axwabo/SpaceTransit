using SpaceTransit.Movement;
using UnityEngine;

namespace SpaceTransit.Ships
{

    [CreateAssetMenu(fileName = "Restart Sequence", menuName = "SpaceTransit/Restart Sequence")]
    public sealed class RestartSequence : ScriptableObject
    {

        [SerializeField]
        private AudioClip start;

        public async Awaitable Execute(ShipController controller)
        {
            var token = controller.Assembly.destroyCancellationToken;
            if (controller.TryGetVaulter(out var vaulter))
                vaulter.Announcer.AnnounceRestarting();
            await Awaitable.WaitForSecondsAsync(10, token);
            // TODO
            AudioSource.PlayClipAtPoint(start, MovementController.Current.Position);
            await Awaitable.WaitForSecondsAsync(10, token);
            FlashAlarms(controller);
            await Awaitable.WaitForSecondsAsync(10, token);
        }

        private static void FlashAlarms(ShipController controller)
        {
            foreach (var module in controller.Assembly.Modules)
            foreach (var door in module.Doors)
                door.Alarm.Flash();
        }

    }

}

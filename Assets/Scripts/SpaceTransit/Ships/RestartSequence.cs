using System.Threading;
using SpaceTransit.Movement;
using SpaceTransit.Vaulter;
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
            AnnounceRestart(controller);
            while (!controller.ModulesReadyForDeparture)
                await Awaitable.NextFrameAsync(token);
            await Awaitable.WaitForSecondsAsync(10, token);
            // TODO
            AudioSource.PlayClipAtPoint(start, MovementController.Current.Position);
            await Awaitable.WaitForSecondsAsync(5, token);
            FlashAlarms(controller);
            await Awaitable.WaitForSecondsAsync(10, token);
            await LoadAsync(controller, token);
            await Awaitable.WaitForSecondsAsync(10, token);
        }

        private static void AnnounceRestart(ShipController controller)
        {
            if (controller.TryGetVaulter(out var vaulter))
                vaulter.Announcer.AnnounceRestarting();
        }

        private void FlashAlarms(ShipController controller)
        {
            foreach (var module in controller.Assembly.Modules)
            foreach (var door in module.Doors)
                door.Alarm.Flash(beep);
        }

        private static async Awaitable LoadAsync(ShipController controller, CancellationToken token)
        {
            if (!controller.TryGetVaulter(out var vaulter))
                return;
            foreach (var screen in vaulter.Screens)
                screen.ShowRestartingProgress();
            var progress = 0f;
            while (progress < 1)
            {
                SetProgress(vaulter, progress);
                progress += Random.Range(0.05f, 0.1f);
                await Awaitable.WaitForSecondsAsync(Random.Range(0, 0.2f), token);
            }

            SetProgress(vaulter, 1);
            await Awaitable.WaitForSecondsAsync(Random.Range(0.5f, 1.5f), token);
        }

        private static void SetProgress(VaulterController vaulter, float progress)
        {
            foreach (var screen in vaulter.Screens)
                screen.RestartingProgress = progress;
        }

    }

}

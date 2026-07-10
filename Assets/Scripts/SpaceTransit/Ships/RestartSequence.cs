using System.Linq;
using System.Threading;
using SpaceTransit.Routes;
using SpaceTransit.Ships.Driving.Screens;
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

        [SerializeField]
        private AudioClip thruster;

        [SerializeField]
        private float thrusterVolume = 0.5f;

        public async Awaitable Execute(ShipController controller, CancellationToken token)
        {
            AnnounceRestart(controller);
            while (!controller.ModulesReadyForDeparture)
                await Awaitable.NextFrameAsync(token);
            await Awaitable.WaitForSecondsAsync(Random.Range(8, 12), token);
            controller.Assembly.FrontModule.Cosmos.RestartSource.PlayOneShot(start);
            controller.Assembly.BackModule.Cosmos.RestartSource.PlayOneShot(start);
            await Awaitable.WaitForSecondsAsync(Random.Range(7, 10), token);
            TriggerThrusters(controller);
            await Awaitable.WaitForSecondsAsync(Random.Range(3, 5), token);
            FlashAlarms(controller);
            await Awaitable.WaitForSecondsAsync(Random.Range(5, 8), token);
            var cosmos = LoadCosmosAsync(controller, token);
            await Awaitable.WaitForSecondsAsync(Random.Range(5, 15), token);
            var vaulter = LoadVaulterAsync(controller, token);
            await AwaitableExtensions.WhenAll(cosmos, vaulter);
            if (controller.Assembly.FrontModule.Thruster.Tube is Dock dock && dock.Station.Announcer)
                dock.Station.Announcer.EnqueueRestarted(controller, dock.Index);
        }

        private static void AnnounceRestart(ShipController controller)
        {
            if (controller.TryGetVaulter(out var vaulter) && vaulter.IsInService)
                vaulter.Announcer.AnnounceRestarting();
            if (controller.Assembly.FrontModule.Thruster.Tube is Dock dock && dock.Station.Announcer)
                dock.Station.Announcer.EnqueueRestarting(controller, dock.Index);
        }

        private void FlashAlarms(ShipController controller)
        {
            foreach (var module in controller.Assembly.Modules)
            foreach (var door in module.Doors)
                door.Alarm.Flash(beep);
        }

        private void TriggerThrusters(ShipController controller)
        {
            foreach (var module in controller.Assembly.Modules)
            foreach (var visualThruster in module.VisualThrusters)
                visualThruster.Trigger(thruster, thrusterVolume);
        }

        private static Awaitable LoadCosmosAsync(ShipController controller, CancellationToken token) => LoadAsync(new[]
        {
            controller.Assembly.FrontModule.Cosmos.Restartable,
            controller.Assembly.BackModule.Cosmos.Restartable
        }, true, token);

        private static async Awaitable LoadVaulterAsync(ShipController controller, CancellationToken token)
        {
            if (!controller.TryGetVaulter(out var vaulter))
                return;
            var screens = vaulter.Screens.Select(e => e.Restartable).ToArray();
            await LoadAsync(screens, false, token);
        }

        private static async Awaitable LoadAsync(RestartableScreen[] screens, bool showScreen, CancellationToken token)
        {
            foreach (var screen in screens)
                screen.SetProgressVisibility(true);
            await Awaitable.WaitForSecondsAsync(Random.Range(0.5f, 1), token);
            var progress = 0f;
            while (progress < 1)
            {
                foreach (var screen in screens)
                    screen.Progress = progress;
                progress += Random.Range(0.05f, 0.1f);
                await Awaitable.WaitForSecondsAsync(Random.Range(0, 0.3f), token);
            }

            foreach (var screen in screens)
                screen.Progress = 1;
            await Awaitable.WaitForSecondsAsync(Random.Range(showScreen ? 0.5f : 2, showScreen ? 1.5f : 4), token);
            foreach (var screen in screens)
            {
                screen.SetProgressVisibility(false);
                screen.SetClass("white", true);
            }

            await Awaitable.WaitForSecondsAsync(Random.Range(1, 2), token);
            foreach (var screen in screens)
            {
                screen.SetClass("white", false);
                screen.SetClass("blue", true);
            }

            await Awaitable.WaitForSecondsAsync(Random.Range(1.5f, 4), token);
            foreach (var screen in screens)
            {
                screen.SetClass("blue", false);
                if (showScreen)
                    screen.EndRestart();
            }
        }

    }

}

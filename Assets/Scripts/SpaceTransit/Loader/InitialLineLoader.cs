using SpaceTransit.Movement;
using SpaceTransit.Routes.Sequences;
using UnityEngine;

namespace SpaceTransit.Loader
{

    public sealed class InitialLineLoader : MonoBehaviour
    {

        [SerializeField]
        private GameObject menu;

        [SerializeField]
        private MovementController player;

        private void Awake()
        {
            menu.SetActive(false);
            player.Awake();
            player.gameObject.SetActive(false);
        }

        private async Awaitable Start()
        {
            var token = WorldChanger.Cts.Token;
            await Resources.UnloadUnusedAssets();

            for (var i = 0; i < MovementController.StartingStation.Lines.Length; i++)
                if (World.LoadScene(MovementController.StartingStation.Lines[i]) is { } operation)
                    await operation;

            await WorldChanger.InitNewScenes(token, true);
            Clock.OffsetSeconds = -Time.timeSinceLevelLoadAsDouble;

            WorldChanger.ActivateNewScenes();

            Destroy(gameObject);
            menu.SetActive(true);
            player.gameObject.SetActive(true);
            player.transform.parent = World.Current;

            await Awaitable.NextFrameAsync(token);
            RouteManager.Start();
            LoadingProgress.Current = null;
        }

    }

}

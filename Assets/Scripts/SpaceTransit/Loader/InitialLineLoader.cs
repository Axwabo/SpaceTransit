using SpaceTransit.Movement;
using SpaceTransit.Routes.Sequences;
using UnityEngine;

namespace SpaceTransit.Loader
{

    public sealed class InitialLineLoader : MonoBehaviour
    {

        [SerializeField]
        private GameObject loader;

        [SerializeField]
        private GameObject menu;

        [SerializeField]
        private GameObject player;

        private void Awake()
        {
            player.SetActive(false);
            menu.SetActive(false);
        }

        private async Awaitable Start()
        {
            var token = WorldChanger.Cts.Token;

            for (var i = 0; i < MovementController.StartingStation.Lines.Length; i++)
                if (World.LoadScene(MovementController.StartingStation.Lines[i]) is { } operation)
                    await operation;

            await WorldChanger.InitNewScenes(token);
            Clock.OffsetSeconds = -Time.timeSinceLevelLoadAsDouble;

            WorldChanger.ActivateNewScenes();

            Destroy(loader);
            menu.SetActive(true);
            player.SetActive(true);
            player.transform.parent = World.Current;
            RouteManager.Current.RefreshLines();
        }

    }

}

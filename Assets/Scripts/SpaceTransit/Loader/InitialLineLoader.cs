using SpaceTransit.Build;
using SpaceTransit.Movement;
using SpaceTransit.Vaulter;
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
            for (var i = 0; i < MovementController.StartingStation.Lines.Length; i++)
                await World.LoadScene(MovementController.StartingStation.Lines[i]);
            var total = 0;
            foreach (var info in SceneInfo.List)
                total += info.Load.Length;
            var progress = new LoadingProgress(total);
            LoadingProgress.Current = progress;
            foreach (var info in SceneInfo.List)
            foreach (var o in info.Load)
            {
                o.SetActive(true);
                progress.Completed++;
                Clock.OffsetSeconds = -Time.timeSinceLevelLoadAsDouble;
                await Awaitable.FixedUpdateAsync();
            }

            foreach (var info in SceneInfo.List)
            {
                foreach (var o in info.Activate)
                    o.SetActive(true);
                Destroy(info);
            }

            SceneInfo.List.Clear();

            Destroy(loader);
            menu.SetActive(true);
            player.SetActive(true);
            player.transform.parent = World.Current;
            RouteManager.Current.RefreshLines();
        }

    }

}

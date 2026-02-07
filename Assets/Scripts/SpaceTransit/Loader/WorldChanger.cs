using SpaceTransit.Build;
using SpaceTransit.Routes.Sequences;
using UnityEngine;

namespace SpaceTransit.Loader
{

    public sealed class WorldChanger : MonoBehaviour
    {

        [SerializeField]
        private int unloadForwards;

        [SerializeField]
        private int unloadBackwards;

        [SerializeField]
        private int loadForwards;

        [SerializeField]
        private int loadBackwards;

        private void OnTriggerEnter(Collider other)
        {
            var t = transform;
            var isBack = Vector3.Dot(t.InverseTransformPoint(other.transform.position).normalized, t.forward) < 0;
            World.Unload(isBack ? unloadForwards : unloadBackwards);
            _ = Load(isBack ? loadForwards : loadBackwards);
        }

        public static async Awaitable Load(params int[] lines)
        {
            LoadingProgress.Current = LoadingProgress.Zero;
            var any = false;
            foreach (var line in lines)
            {
                if (World.LoadScene(line) is not { } operation)
                    continue;
                any = true;
                await operation;
            }

            if (!any)
            {
                LoadingProgress.Current = null;
                return;
            }

            while (SceneInfo.List.Count == 0)
                await Awaitable.NextFrameAsync();

            await InitNewScenes();

            ActivateNewScenes();
            RouteManager.Current.RefreshLines();
        }

        public static async Awaitable InitNewScenes()
        {
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
                await Awaitable.FixedUpdateAsync();
            }
        }

        public static void ActivateNewScenes()
        {
            foreach (var info in SceneInfo.List)
            {
                foreach (var o in info.Activate)
                    o.SetActive(true);
                Destroy(info);
            }

            SceneInfo.List.Clear();
        }

    }

}

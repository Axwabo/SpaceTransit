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

        public static async Awaitable Load(int line)
        {
            if (World.LoadScene(line) is not { } operation)
                return;
            LoadingProgress.Current = LoadingProgress.Zero;
            await operation;
            while (SceneInfo.List.Count == 0)
                await Awaitable.NextFrameAsync();
            var timeScale = Time.timeScale;
            var info = SceneInfo.List.FirstFast();
            var progress = new LoadingProgress(info.Load.Length);
            LoadingProgress.Current = progress;
            Time.timeScale = 1;
            foreach (var o in info.Load)
            {
                o.SetActive(true);
                progress.Completed++;
                await Awaitable.NextFrameAsync();
            }

            foreach (var o in info.Activate)
                o.SetActive(true);

            Time.timeScale = timeScale;
            SceneInfo.List.Remove(info);
            Destroy(info);
            RouteManager.Current.RefreshLines();
        }

    }

}

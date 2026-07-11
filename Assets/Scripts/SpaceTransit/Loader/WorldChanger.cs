using System;
using System.Linq;
using System.Threading;
using SpaceTransit.Build;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SpaceTransit.Loader
{

    public sealed class WorldChanger : MonoBehaviour
    {

        public static CancellationTokenSource Cts { get; private set; } = new();

        public static event Action SceneFullyLoaded;

        [SerializeField]
        [HideInInspector]
        private string[] unloadFrontNames;

        [SerializeField]
        [HideInInspector]
        private string[] unloadBackNames;

        [SerializeField]
        [HideInInspector]
        private string[] loadFrontNames;

        [SerializeField]
        [HideInInspector]
        private string[] loadBackNames;

#if UNITY_EDITOR
        [SerializeField]
        private SceneAsset[] unloadFront;

        [SerializeField]
        private SceneAsset[] unloadBack;

        [SerializeField]
        private SceneAsset[] loadFront;

        [SerializeField]
        private SceneAsset[] loadBack;

        private void OnValidate()
        {
            Convert(unloadFront, out unloadFrontNames);
            Convert(unloadBack, out unloadBackNames);
            Convert(loadFront, out loadFrontNames);
            Convert(loadBack, out loadBackNames);
        }

        private static void Convert(SceneAsset[] assets, out string[] names) => names = assets.Where(e => e).Select(e => e.name).ToArray();
#endif

        private void OnTriggerEnter(Collider other)
        {
            if (LoadingProgress.Current != null)
                return;
            var t = transform;
            var isBack = Vector3.Dot((other.transform.position - t.position).normalized, t.forward) < 0;
            World.Unload(isBack ? unloadFrontNames : unloadBackNames);
            _ = Load(isBack ? loadFrontNames : loadBackNames);
        }

        public static async Awaitable Load(params string[] lines)
        {
            var token = Cts.Token;
            LoadingProgress.Current = LoadingProgress.Zero;
            var any = false;
            foreach (var line in lines)
            {
                if (World.LoadScene(line) is not { } operation)
                    continue;
                Time.timeScale = 1;
                any = true;
                await operation;
                SceneFullyLoaded?.Invoke();
            }

            if (!any)
            {
                LoadingProgress.Current = null;
                return;
            }

            while (SceneInfo.List.Count == 0)
                await Awaitable.NextFrameAsync(token);

            await InitNewScenes(token);

            ActivateNewScenes();
        }

        public static async Awaitable InitNewScenes(CancellationToken token, bool fast = false)
        {
            token.ThrowIfCancellationRequested();
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
                if (fast)
                    await Awaitable.FixedUpdateAsync(token);
                else
                    await Awaitable.NextFrameAsync(token);
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

        public static void Cancel()
        {
            SceneInfo.List.Clear();
            Cts.Cancel();
            Cts.Dispose();
            Cts = new CancellationTokenSource();
            LoadingProgress.Current = null;
        }

    }

}

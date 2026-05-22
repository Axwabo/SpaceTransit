using System;
using System.Threading;
using SpaceTransit.Build;
using UnityEditor;
using UnityEngine;

namespace SpaceTransit.Loader
{

    public sealed class WorldChanger : MonoBehaviour
    {

        public static CancellationTokenSource Cts { get; private set; } = new();

        [Obsolete, SerializeField, HideInInspector]
        private string unloadForwards;

        [Obsolete, SerializeField, HideInInspector]
        private string unloadBackwards;

        [Obsolete, SerializeField, HideInInspector]
        private string loadForwards;

        [Obsolete, SerializeField, HideInInspector]
        private string loadBackwards;

        [SerializeField]
        private string[] unloadFront;

        [SerializeField]
        private string[] unloadBack;

        [SerializeField]
        private string[] loadFront;

        [SerializeField]
        private string[] loadBack;

        [Obsolete, ContextMenu("Migrate to array")]
        private void Migrate()
        {
            unloadFront = ToArray(unloadForwards);
            unloadBack = ToArray(unloadBackwards);
            loadFront = ToArray(loadForwards);
            loadBack = ToArray(loadBackwards);
            EditorUtility.SetDirty(this);
        }

        private static string[] ToArray(string s) => string.IsNullOrEmpty(s) ? Array.Empty<string>() : new[] {s};

        private void OnTriggerEnter(Collider other)
        {
            if (LoadingProgress.Current != null)
                return;
            var t = transform;
            var isBack = Vector3.Dot((other.transform.position - t.position).normalized, t.forward) < 0;
            World.Unload(isBack ? unloadFront : unloadBack);
            _ = Load(isBack ? loadFront : loadBack);
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

        public static async Awaitable InitNewScenes(CancellationToken token)
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
                await Awaitable.FixedUpdateAsync(token);
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

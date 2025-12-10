#if UNITY_EDITOR
using System.Collections.Generic;
using SplineMesh;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceTransit.Build
{

    public sealed class GradualTubeLoaderProcessor : IProcessSceneWithReport
    {

        public int callbackOrder => 0;

        public void OnProcessScene(Scene scene, BuildReport report)
        {
            var rootGameObjects = scene.GetRootGameObjects();
            if (!TryFindProgress(rootGameObjects, out var progress))
                return;
            var splines = new List<Spline>();
            var activates = new List<Activate>();
            var load = new List<GameObject>();
            var activate = new List<GameObject>();
            foreach (var root in rootGameObjects)
            {
                root.GetComponentsInChildren(splines);
                root.GetComponentsInChildren(activates);
                foreach (var spline in splines)
                {
                    var go = spline.gameObject;
                    load.Add(go);
                    go.SetActive(false);
                }

                foreach (var instance in activates)
                {
                    var go = instance.gameObject;
                    activate.Add(go);
                    go.SetActive(false);
                    Object.Destroy(instance);
                }
            }

            if (activate.Count == 0)
                return;
            var loader = rootGameObjects[0].AddComponent<GradualTubeLoader>();
            loader.Load = load.ToArray();
            loader.Activate = activate.ToArray();
            loader.ProgressContainer = progress;
        }

        private static bool TryFindProgress(GameObject[] rootGameObjects, out GameObject progress)
        {
            foreach (var root in rootGameObjects)
            {
                if (!root.TryGetComponent(out ProgressDisplay _))
                    continue;
                progress = root;
                return true;
            }

            progress = null;
            return false;
        }

    }

}
#endif

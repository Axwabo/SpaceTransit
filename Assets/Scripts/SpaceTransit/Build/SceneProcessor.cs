#if UNITY_EDITOR
using System.Collections.Generic;
using SplineMesh;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceTransit.Build
{

    public sealed class SceneProcessor : IProcessSceneWithReport
    {

        public int callbackOrder => 0;

        public void OnProcessScene(Scene scene, BuildReport report)
        {
            var rootGameObjects = scene.GetRootGameObjects();
            if (!TryFindProgress(rootGameObjects, out var progress))
                return;
            var splines = new List<Spline>();
            var hoist = new List<HoistColliders>();
            var activates = new List<Activate>();
            var load = new List<GameObject>();
            var activate = new List<GameObject>();
            var hoistTransforms = new List<MeshCollider>();
            foreach (var root in rootGameObjects)
                ProcessRoot(root, splines, activates, load, activate, hoist, hoistTransforms);
            if (activate.Count == 0)
                return;
            var loader = rootGameObjects[0].AddComponent<GradualTubeLoader>();
            loader.Load = load.ToArray();
            loader.Activate = activate.ToArray();
            loader.ProgressContainer = progress;
        }

        private static void ProcessRoot(GameObject root, List<Spline> splines, List<Activate> activates, List<GameObject> load, List<GameObject> activate, List<HoistColliders> hoist, List<MeshCollider> hoistTransforms)
        {
            root.GetComponentsInChildren(splines);
            root.GetComponentsInChildren(activates);
            root.GetComponentsInChildren(hoist);
            foreach (var spline in splines)
            {
                var go = spline.gameObject;
                load.Add(go);
                go.SetActive(false);
            }

            foreach (var instance in hoist)
            {
                var parent = instance.transform.parent;
                instance.GetComponentsInChildren(hoistTransforms);
                foreach (var collider in hoistTransforms)
                    collider.transform.parent = parent;
            }

            foreach (var instance in activates)
            {
                var go = instance.gameObject;
                activate.Add(go);
                go.SetActive(false);
                Object.DestroyImmediate(instance);
            }
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

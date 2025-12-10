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
            var results = new List<Spline>();
            var load = new List<GameObject>();
            foreach (var root in scene.GetRootGameObjects())
            {
                root.GetComponentsInChildren(results);
                foreach (var spline in results)
                {
                    var go = spline.gameObject;
                    load.Add(go);
                    go.SetActive(false);
                }

                if (load.Count != 0)
                    root.AddComponent<GradualTubeLoader>().TubesToLoad = load.ToArray();
            }
        }

    }

}
#endif

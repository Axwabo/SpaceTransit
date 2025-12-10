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
            var splines = new List<Spline>();
            var activates = new List<Activate>();
            var load = new List<GameObject>();
            var activate = new List<GameObject>();
            var rootGameObjects = scene.GetRootGameObjects();
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

            load.AddRange(activate);
            rootGameObjects[0].AddComponent<GradualTubeLoader>().Load = load.ToArray();
        }

    }

}
#endif

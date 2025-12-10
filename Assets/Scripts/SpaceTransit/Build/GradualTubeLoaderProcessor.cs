#if UNITY_EDITOR
using System.Collections.Generic;
using SplineMesh;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine.SceneManagement;

namespace SpaceTransit.Build
{

    public sealed class GradualTubeLoaderProcessor : IProcessSceneWithReport
    {

        public int callbackOrder => 0;

        public void OnProcessScene(Scene scene, BuildReport report)
        {
            var results = new List<Spline>();
            var load = new List<TubeToLoad>();
            foreach (var root in scene.GetRootGameObjects())
            {
                root.GetComponentsInChildren(results);
                foreach (var spline in results)
                {
                    if (!spline.TryGetComponent(out SplineMeshTiling tiling))
                        continue;
                    load.Add(new TubeToLoad
                    {
                        spline = spline,
                        tiling = tiling
                    });
                    spline.enabled = false;
                    tiling.enabled = false;
                }

                if (load.Count != 0)
                    root.AddComponent<GradualTubeLoader>().TubesToLoad = load.ToArray();
            }
        }

    }

}
#endif

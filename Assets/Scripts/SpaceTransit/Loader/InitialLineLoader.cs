using System.Collections.Generic;
using SpaceTransit.Movement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceTransit.Loader
{

    public sealed class InitialLineLoader : MonoBehaviour
    {

        private void Start()
        {
            var buildIndices = new Dictionary<int, int>();
            var sceneCount = SceneManager.sceneCountInBuildSettings;
            for (var i = 0; i < sceneCount; i++)
            {
                var scene = SceneManager.GetSceneByBuildIndex(i);
                if (int.TryParse(scene.name, out var line))
                    buildIndices[line] = scene.buildIndex;
            }

            foreach (var line in MovementController.StartingStation.Lines)
                if (buildIndices.TryGetValue(line, out var index))
                    SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        }

    }

}

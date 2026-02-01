using SpaceTransit.Movement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceTransit.Loader
{

    public sealed class InitialLineLoader : MonoBehaviour
    {

        private void Start()
        {
            foreach (var line in MovementController.StartingStation.Lines)
                SceneManager.LoadSceneAsync(line.ToString(), LoadSceneMode.Additive);
        }

    }

}

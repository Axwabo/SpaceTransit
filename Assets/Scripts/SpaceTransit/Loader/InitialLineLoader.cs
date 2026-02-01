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
            {
                var operation = SceneManager.LoadSceneAsync(line.ToString(), LoadSceneMode.Additive);
                if (operation != null)
                    operation.completed += OperationOnCompleted;
            }
        }

        private void OperationOnCompleted(AsyncOperation obj)
        {
            Debug.Log("completed");
        }

    }

}

using SpaceTransit.Movement;
using UnityEngine;

namespace SpaceTransit.Loader
{

    public sealed class InitialLineLoader : MonoBehaviour
    {

        private void Start()
        {
            foreach (var line in MovementController.StartingStation.Lines)
                World.Load(line);
        }

    }

}

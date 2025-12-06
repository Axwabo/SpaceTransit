using SpaceTransit.Movement;
using SpaceTransit.Ships.Modules;
using UnityEngine;
using UnityEngine.Audio;

namespace SpaceTransit.Audio
{

    public sealed class MuffleController : ShipComponentBase
    {

        [SerializeField]
        private float minDoorDistance;

        [SerializeField]
        private float maxDoorDistance;

        [SerializeField]
        private AudioMixer mixer;

        [SerializeField]
        private AnimationCurve volume;

        [SerializeField]
        private AnimationCurve cutoff;

        [SerializeField]
        private string volumeParameter;

        [SerializeField]
        private string cutoffParameter;

        private void LateUpdate()
        {
            if (!Assembly.IsPlayerMounted)
                return;
            var volumeScore = 0f;
            var cutoffScore = 0f;
            var position = MovementController.Current.LastPosition;
            foreach (var module in Assembly.Modules)
            foreach (var door in module.Doors)
            {
                if (door.CanDepart)
                    continue;
                var distance = Vector3.Distance(position, door.Transform.position) - minDoorDistance;
                volumeScore += (1 - Mathf.Clamp01(distance / maxDoorDistance)) * (1 - Mathf.Pow(1 - door.Openness, 2));
                cutoffScore += door.Openness;
            }

            mixer.SetFloat(volumeParameter, volume.Evaluate(volumeScore));
            mixer.SetFloat(cutoffParameter, cutoff.Evaluate(cutoffScore));
        }

    }

}

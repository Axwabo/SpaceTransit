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
            var score = 0f;
            var position = MovementController.Current.LastPosition;
            foreach (var module in Assembly.Modules)
            foreach (var door in module.Doors)
                if (door.CanDepart)
                    score += 1 - Mathf.Clamp01((Vector3.Distance(position, door.Transform.position) + minDoorDistance) / maxDoorDistance);
            mixer.SetFloat(volumeParameter, volume.Evaluate(score));
            mixer.SetFloat(cutoffParameter, cutoff.Evaluate(score));
        }

    }

}

using SpaceTransit.Movement;
using UnityEngine;
using UnityEngine.Audio;

namespace SpaceTransit.Audio
{

    [RequireComponent(typeof(AudioSource))]
    public sealed class MountBasedMixerSetter : MonoBehaviour
    {

        private AudioSource _source;

        private AudioMixerGroup _defaultGroup;

        private bool _wasMounted;

        [SerializeField]
        private AudioMixerGroup onboardGroup;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
            _defaultGroup = _source.outputAudioMixerGroup;
        }

        private void Update()
        {
            var mounted = MovementController.Current.IsMounted;
            if (mounted == _wasMounted)
                return;
            _wasMounted = mounted;
            _source.outputAudioMixerGroup = mounted ? onboardGroup : _defaultGroup;
        }

    }

}

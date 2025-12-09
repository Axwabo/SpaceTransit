using SpaceTransit.Movement;
using SpaceTransit.Ships.Modules;
using UnityEngine;
using UnityEngine.Audio;

namespace SpaceTransit.Audio
{

    [RequireComponent(typeof(AudioSource))]
    public sealed class MountBasedMixerSetter : ModuleComponentBase
    {

        private AudioSource _source;

        private AudioMixerGroup _defaultGroup;

        private bool _wasMounted;

        private bool _hasParent;

        [SerializeField]
        private AudioMixerGroup onboardGroup;

        protected override void Awake()
        {
            _source = GetComponent<AudioSource>();
            _defaultGroup = _source.outputAudioMixerGroup;
        }

        private void LateUpdate()
        {
            var mounted = MovementController.Current.IsMounted && (!_hasParent || !Assembly.IsPlayerMounted);
            if (mounted == _wasMounted)
                return;
            _wasMounted = mounted;
            _source.outputAudioMixerGroup = mounted ? onboardGroup : _defaultGroup;
        }

        protected override void OnInitialized() => _hasParent = true;

    }

}

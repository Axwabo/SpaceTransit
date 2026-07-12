using SpaceTransit.Movement;
using SpaceTransit.Ships.Modules;
using UnityEngine;
using UnityEngine.Audio;

namespace SpaceTransit.Audio
{

    [RequireComponent(typeof(AudioSource))]
    public sealed class MountBasedMixerSetter : ModuleComponentBase, IShipAudioComponent
    {

        public AudioSource Source { get; private set; }

        public bool Mute { get; private set; }

        private AudioMixerGroup _defaultGroup;

        private bool _defaultMute;

        private bool? _wasMounted;

        private bool _hasParent;

        [SerializeField]
        private AudioMixerGroup onboardGroup;

        [SerializeField]
        private bool onboardMute;

        protected override void Awake()
        {
            Source = GetComponent<AudioSource>();
            _defaultGroup = Source.outputAudioMixerGroup;
            _defaultMute = Source.mute;
        }

        private void OnEnable() => _wasMounted = null;

        private void LateUpdate()
        {
            var mounted = MovementController.Current.IsMounted && (!_hasParent || !Assembly.IsPlayerMounted);
            if (mounted == _wasMounted)
                return;
            _wasMounted = mounted;
            Mute = mounted ? onboardMute : _defaultMute;
            Source.outputAudioMixerGroup = mounted ? onboardGroup : _defaultGroup;
            Source.mute |= Mute;
        }

        protected override void OnInitialized() => _hasParent = true;

    }

}

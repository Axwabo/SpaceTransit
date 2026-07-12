using SpaceTransit.Movement;
using SpaceTransit.Ships.Modules;
using UnityEngine;
using UnityEngine.Audio;

namespace SpaceTransit.Audio
{

    [RequireComponent(typeof(AudioSource))]
    public sealed class MountBasedMixerSetter : ModuleComponentBase, IShipAudioComponent
    {

        private AudioSource _source;

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
            _source = GetComponent<AudioSource>();
            _defaultGroup = _source.outputAudioMixerGroup;
            _defaultMute = _source.mute;
        }

        private void OnEnable() => _wasMounted = null;

        private void LateUpdate() => UpdateMuteStatus();

        public void UpdateMuteStatus()
        {
            var mounted = MovementController.Current.IsMounted && (!_hasParent || !Assembly.IsPlayerMounted);
            if (mounted == _wasMounted)
                return;
            _wasMounted = mounted;
            Mute = mounted ? onboardMute : _defaultMute;
            _source.outputAudioMixerGroup = mounted ? onboardGroup : _defaultGroup;
        }

        protected override void OnInitialized() => _hasParent = true;

    }

}

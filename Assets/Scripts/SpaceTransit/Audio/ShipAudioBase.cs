using System.Collections.Generic;
using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Audio
{

    public abstract class ShipAudioBase : ShipComponentBase
    {

        private AudioSource[] _sources;

        private readonly Dictionary<AudioSource, IShipAudioComponent> _muteControllers = new();

        protected bool IsPlayerMounted => Assembly.IsPlayerMounted;

        protected bool Mute
        {
            set
            {
                foreach (var source in _sources)
                    source.mute = value && (!_muteControllers.TryGetValue(source, out var controller) || controller.Mute);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _sources = GetComponentsInChildren<AudioSource>();
            foreach (var source in _sources)
                if (source.TryGetComponent(out IShipAudioComponent component))
                    _muteControllers[source] = component;
        }

    }

}

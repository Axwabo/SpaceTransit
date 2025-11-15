using System.Linq;
using SpaceTransit.Movement;
using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Audio
{

    public abstract class ShipAudioBase : ShipComponentBase
    {

        protected AudioSource[] Sources { get; private set; }

        protected bool IsPlayerMounted => Assembly.Modules.Any(e => e.Mount.Transform == MovementController.Current.Mount);

        protected bool Mute
        {
            set
            {
                foreach (var source in Sources)
                    source.mute = value;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            Sources = GetComponentsInChildren<AudioSource>();
        }

    }

}

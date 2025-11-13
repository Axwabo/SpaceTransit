using System.Linq;
using SpaceTransit.Movement;
using SpaceTransit.Ships;
using UnityEngine;

namespace SpaceTransit.Audio
{

    public abstract class ShipAudioBase : MonoBehaviour
    {

        protected Transform Transform { get; private set; }

        protected ShipAssembly Assembly { get; private set; }

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

        private void Awake()
        {
            Transform = transform;
            Assembly = GetComponentInParent<ShipAssembly>();
            Sources = GetComponentsInChildren<AudioSource>();
        }

    }

}

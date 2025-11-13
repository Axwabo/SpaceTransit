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

        protected AudioSource Source { get; set; }

        protected bool IsPlayerMounted => Assembly.Modules.Any(e => e.Mount.Transform == MovementController.Current.Mount);

        private void Awake()
        {
            Transform = transform;
            Assembly = GetComponentInParent<ShipAssembly>();
            Source = GetComponent<AudioSource>();
        }

    }

}

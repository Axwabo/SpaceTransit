using System.Linq;
using SpaceTransit.Movement;
using SpaceTransit.Ships;
using UnityEngine;

namespace SpaceTransit.Audio
{

    [RequireComponent(typeof(AudioSource))]
    public sealed class OutsideAudio : MonoBehaviour
    {

        private Transform _t;

        private ShipAssembly _assembly;

        private AudioSource _source;

        private bool IsPlayerMounted => _assembly.Modules.Any(e => e.Mount.Transform == MovementController.Current.Mount);

        private void Awake()
        {
            _t = transform;
            _assembly = GetComponentInParent<ShipAssembly>();
            _source = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (IsPlayerMounted)
            {
                _source.mute = true;
                return;
            }

            _source.mute = false;
            _t.position = _assembly.ClosestPoint(MovementController.Current.Position);
        }

    }

}

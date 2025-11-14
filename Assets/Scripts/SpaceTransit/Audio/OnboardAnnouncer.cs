using SpaceTransit.Ships;
using UnityEngine;

namespace SpaceTransit.Audio
{

    [RequireComponent(typeof(QueuePlayer))]
    public sealed class OnboardAnnouncer : MonoBehaviour
    {

        [SerializeField]
        private AudioClip signal;

        private ShipController _controller;

        private QueuePlayer _player;

        private bool _wasSailing;

        private float _delay;

        private void Awake()
        {
            _controller = GetComponentInParent<ShipController>();
            _player = GetComponent<QueuePlayer>();
        }

        private void Update()
        {
            if (_controller.State != ShipState.Sailing)
            {
                _wasSailing = false;
                return;
            }

            if (!_wasSailing)
                _delay = 10;
            _wasSailing = true;
            if (_delay > 0 && (_delay -= Time.deltaTime) <= 0)
            {
                _player.Enqueue(signal);
                // play "next station is ..."
            }
            // check if near station
        }

    }

}

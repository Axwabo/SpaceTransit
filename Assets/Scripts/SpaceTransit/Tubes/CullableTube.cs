using System;
using SpaceTransit.Movement;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpaceTransit.Tubes
{

    public sealed class CullableTube : MonoBehaviour
    {

        private const float MaxDistance = 4000;

        private int _wait;

        private MeshRenderer _renderer;

        private Vector3 _center;

        private bool _previousState = true;

        private void Awake() => _wait = Random.Range(1, 3);

        private void Start()
        {
            var tube = GetComponent<TubeBase>();
            _renderer = GetComponentInChildren<MeshRenderer>();
            _center = tube.Sample(tube.Length * 0.5f).Position;
            SetState();
        }

        private void Update()
        {
            if (--_wait > 0)
                return;
            _wait = 10;
            SetState();
        }

        private void SetState()
        {
            var player = World.Current.InverseTransformPoint(MovementController.Current.LastPosition);
            var visible = Vector3.Distance(_center, player) < MaxDistance;
            if (_previousState == visible)
                return;
            _renderer.enabled = visible;
            _previousState = visible;
        }

    }

}

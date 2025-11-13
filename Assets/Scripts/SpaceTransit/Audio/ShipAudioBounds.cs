using System;
using SpaceTransit.Ships;
using UnityEngine;

namespace SpaceTransit.Audio
{

    [RequireComponent(typeof(ShipAssembly))]
    public sealed class ShipAudioBounds : MonoBehaviour
    {

        private ShipAssembly _assembly;

        private Vector3[] _closestPoints;

        private void Awake() => _assembly = GetComponent<ShipAssembly>();

        private void Start() => _closestPoints = new Vector3[_assembly.Modules.Count];

        public Vector3 ClosestPoint(Vector3 position)
        {
            for (var i = 0; i < _assembly.Modules.Count; i++)
                _closestPoints[i] = _assembly.Modules[i].AudioBounds.Closest(position);
            Array.Sort(_closestPoints, SqrMagnitudeComparer.Instance);
            return _closestPoints[0];
        }

    }

}

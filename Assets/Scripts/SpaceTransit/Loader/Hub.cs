using SpaceTransit.Movement;
using SpaceTransit.Routes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpaceTransit.Loader
{

    [RequireComponent(typeof(Station))]
    public sealed class Hub : MonoBehaviour
    {

        private StationId _id;

        private Transform _t;

        private float _cooldown;

        private bool _wasNear;

        private void Awake()
        {
            _id = GetComponent<Station>().ID;
            _t = transform;
            _cooldown = Random.value * 3 + 2;
            MovementController.Teleported += UpdateState;
        }

        private void Update()
        {
            if ((_cooldown -= Clock.Delta) <= 0)
                UpdateState();
        }

        private void OnDestroy() => MovementController.Teleported -= UpdateState;

        private void UpdateState()
        {
            _cooldown = 5;
            var pos = MovementController.Current.LastPosition;
            var isNear = Vector3.Distance(_t.position, pos) < 30;
            if (isNear == _wasNear)
                return;
            _wasNear = isNear;
            if (!isNear)
                return;
            foreach (var line in _id.Lines)
                _ = World.Load(line);
        }

    }

}

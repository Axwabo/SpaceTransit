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

        private float _remaining;

        private void Awake()
        {
            _id = GetComponent<Station>().ID;
            _t = transform;
            _remaining = Random.value * 2 + 3;
        }

        private void Update()
        {
            if ((_remaining -= Time.deltaTime) > 0)
                return;
            _remaining = 5;
            var near = Vector3.Distance(MovementController.Current.LastPosition, _t.position) > 30;
            if (near)
                return;
            // TODO: i've just realized the implications of this
        }

    }

}

using System.Collections.Generic;
using System.Linq;
using SpaceTransit.Ships.Modules;
using UnityEngine;

namespace SpaceTransit.Ships
{

    public sealed class ShipAssembly : MonoBehaviour
    {

        public IReadOnlyList<ShipModule> Modules { get; private set; }

        private void Awake()
        {
            Modules = this.GetComponentsInImmediateChildren<ShipModule>().ToArray();
            if (Modules.Count == 0)
                throw new MissingComponentException("Ships must have at least 1 module");
        }

        private void Start()
        {
            for (var i = 0; i < Modules.Count - 1; i++)
            {
                var joint = Modules[i].gameObject.AddComponent<HingeJoint>();
                joint.connectedBody = Modules[i + 1].Rigidbody;
                joint.axis = Vector3.up;
            }
        }

    }

}

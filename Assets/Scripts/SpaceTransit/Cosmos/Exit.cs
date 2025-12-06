using System.Collections.Generic;
using System.Linq;
using SpaceTransit.Routes;
using SpaceTransit.Ships;
using SpaceTransit.Tubes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpaceTransit.Cosmos
{

    public sealed class Exit : MonoBehaviour
    {

        [field: FormerlySerializedAs("<ConnectedStation>k__BackingField")]
        [field: SerializeField]
        public Station ExitTowards { get; private set; }

        [field: SerializeField]
        public Station EnterTowards { get; private set; }

        [FormerlySerializedAs("connectTube")]
        [SerializeField]
        private TubeBase connectExitTube;

        [FormerlySerializedAs("connectTo")]
        [SerializeField]
        private TubeBase connectExitTo;

        [SerializeField]
        private TubeBase connectEntryTube;

        [SerializeField]
        private TubeBase connectEntryTo;

        public HashSet<ShipAssembly> UsedBy { get; } = new();

        public bool Lock(ShipAssembly assembly, bool exiting)
        {
            if (UsedBy.Count != 0 && !UsedBy.Contains(assembly))
                return false;
            if (!UsedBy.Add(assembly))
                return true;
            if (exiting)
                Connect(connectExitTube, connectExitTo, assembly);
            else
                Connect(connectEntryTube, connectEntryTo, assembly);
            return true;
        }

        private void Connect(TubeBase which, TubeBase to, ShipAssembly assembly)
        {
            if (!to)
                return;
            if (assembly.Reverse)
                which.Previous = to;
            else
                which.Next = to;
        }

        [ContextMenu("Request Entry")]
        private void Enter()
        {
            if (!Lock(ShipAssembly.Instances.First(), false))
                EditorUtility.DisplayDialog("Entry Failed", "Couldn't request entry", "kurwa");
        }

        private void Update() => UsedBy.RemoveWhere(e => !e);

    }

}

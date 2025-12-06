using System.Collections.Generic;
using System.Linq;
using SpaceTransit.Routes;
using SpaceTransit.Ships;
using SpaceTransit.Tubes;
using UnityEditor;
using UnityEngine;

namespace SpaceTransit.Cosmos
{

    public sealed class Entry : MonoBehaviour
    {

        [field: SerializeField]
        public Station ConnectedStation { get; private set; }

        [SerializeField]
        private Exit exit;

        [SerializeField]
        private TubeBase connectTube;

        [SerializeField]
        private TubeBase connectTo;

        public HashSet<ShipAssembly> UsedBy { get; } = new();

        public bool Lock(ShipAssembly assembly)
        {
            if (!exit.Lock(assembly) || UsedBy.Count != 0 && !UsedBy.Contains(assembly))
                return false;
            UsedBy.Add(assembly);
            if (!connectTo)
                return true;
            connectTube.SetNext(connectTo);
            return true;
        }

        [ContextMenu("Request Entry")]
        public void RequestEntry()
        {
            if (!Lock(ShipAssembly.Instances.First()))
                EditorUtility.DisplayDialog("lock failed", "Couldn't request entry", "kurwa");
        }

        private void Update() => UsedBy.RemoveWhere(e => !e);

    }

}

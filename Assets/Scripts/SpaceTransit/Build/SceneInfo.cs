using System.Collections.Generic;
using UnityEngine;

namespace SpaceTransit.Build
{

    public sealed class SceneInfo : MonoBehaviour
    {

        public static HashSet<SceneInfo> List { get; } = new();

        [field: SerializeField]
        public GameObject[] Load { get; set; }

        [field: SerializeField]
        public GameObject[] Activate { get; set; }

        private void Awake() => List.Add(this);

    }

}

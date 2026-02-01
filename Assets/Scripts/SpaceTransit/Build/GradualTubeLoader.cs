using System.Collections.Generic;
using UnityEngine;

namespace SpaceTransit.Build
{

    public sealed class GradualTubeLoader : MonoBehaviour
    {

        public static HashSet<GradualTubeLoader> Instances { get; } = new();

        [field: SerializeField]
        public GameObject[] Load { get; set; }

        [field: SerializeField]
        public GameObject[] Activate { get; set; }

        public int Index { get; private set; }

        private void Awake() => Instances.Add(this);

        private void FixedUpdate()
        {
            Load[Index].SetActive(true);
            if (++Index >= Load.Length)
                Complete();
            Clock.OffsetSeconds = -Time.timeSinceLevelLoadAsDouble;
        }

        private void Complete()
        {
            Destroy(this);
            foreach (var o in Activate)
                o.SetActive(true);
        }

        private void OnDestroy() => Instances.Remove(this);

    }

}

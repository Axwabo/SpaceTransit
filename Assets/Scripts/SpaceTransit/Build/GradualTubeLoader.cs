using UnityEngine;

namespace SpaceTransit.Build
{

    public sealed class GradualTubeLoader : MonoBehaviour
    {

        public static float Progress { get; private set; }

        [field: SerializeField]
        public GameObject[] Load { get; set; }

        [field: SerializeField]
        public GameObject[] Activate { get; set; }

        [field: SerializeField]
        public GameObject ProgressContainer { get; set; }

        private int _index;

        private float _delay = 1;

        private void Awake() => Progress = 0;

        private void FixedUpdate()
        {
            if ((_delay -= Time.fixedDeltaTime) > 0)
                return;
            Load[_index].SetActive(true);
            if (++_index >= Load.Length)
                Complete();
            Progress = (float) _index / Load.Length;
            Clock.OffsetSeconds = Time.timeSinceLevelLoadAsDouble;
        }

        private void Complete()
        {
            Destroy(this);
            if (ProgressContainer)
                Destroy(ProgressContainer);
            foreach (var o in Activate)
                o.SetActive(true);
        }

    }

}

using SpaceTransit.Loader;
using UnityEngine;

namespace SpaceTransit.Build
{

    public sealed class GradualTubeLoader : MonoBehaviour
    {

        [field: SerializeField]
        public GameObject[] Load { get; set; }

        [field: SerializeField]
        public GameObject[] Activate { get; set; }

        private int _index;

        private LoadingProgress _progress;

        private void Start()
        {
            _progress = new LoadingProgress(Load.Length);
            ProgressDisplay.Reports.Add(_progress);
        }

        private void FixedUpdate()
        {
            Load[_index].SetActive(true);
            if (++_index >= Load.Length)
                Complete();
            _progress.Current = _index;
            Clock.OffsetSeconds = -Time.timeSinceLevelLoadAsDouble;
        }

        private void Complete()
        {
            Destroy(this);
            foreach (var o in Activate)
                o.SetActive(true);
        }

    }

}

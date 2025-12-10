using UnityEngine;

namespace SpaceTransit.Build
{

    public sealed class GradualTubeLoader : MonoBehaviour
    {

        public static float Progress { get; private set; }

        [field: SerializeField]
        public TubeToLoad[] TubesToLoad { get; set; }

        private int _index;

        private float _delay = 1;

        private void Awake() => Progress = 0;

        private void Update()
        {
            if ((_delay -= Time.deltaTime) > 0)
                return;
            _delay = 0.5f;
            var load = TubesToLoad[_index];
            load.spline.enabled = true;
            load.tiling.enabled = true;
            if (++_index >= TubesToLoad.Length)
                Destroy(this);
            Progress = (float) _index / TubesToLoad.Length;
        }

    }

}

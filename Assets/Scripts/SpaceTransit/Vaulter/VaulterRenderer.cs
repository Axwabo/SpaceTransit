using UnityEngine;

namespace SpaceTransit.Vaulter
{

    public sealed class VaulterRenderer : VaulterComponentBase
    {

        [SerializeField]
        private int width;

        [SerializeField]
        private int height;

        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private Camera cam;

        public RenderTexture Texture { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Texture = RenderTexture.GetTemporary(width, height, 1);
            cam.targetTexture = Texture;
            canvas.worldCamera = cam;
        }

        private void Start() => Transform.SetParent(null, false);

        private void OnDestroy()
        {
            RenderTexture.ReleaseTemporary(Texture);
            Parent.Renderers.Remove(name);
        }

        protected override void OnInitialized() => Parent.Renderers[name] = this;

    }

}

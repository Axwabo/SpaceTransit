using UnityEngine;

namespace SpaceTransit.Vaulter
{

    public sealed class VaulterRenderer : MonoBehaviour
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

        private void Awake()
        {
            Texture = RenderTexture.GetTemporary(width, height);
            cam.targetTexture = Texture;
            canvas.worldCamera = cam;
            transform.parent = null;
        }

        private void OnDestroy() => RenderTexture.ReleaseTemporary(Texture);

    }

}

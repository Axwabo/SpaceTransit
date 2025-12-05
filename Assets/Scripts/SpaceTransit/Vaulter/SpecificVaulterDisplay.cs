using UnityEngine;
using UnityEngine.UI;

namespace SpaceTransit.Vaulter
{

    [RequireComponent(typeof(RawImage))]
    public sealed class SpecificVaulterDisplay : MonoBehaviour
    {

        [SerializeField]
        private VaulterRenderer vaulterRenderer;

        private void Start() => GetComponent<RawImage>().texture = vaulterRenderer.Texture;

    }

}

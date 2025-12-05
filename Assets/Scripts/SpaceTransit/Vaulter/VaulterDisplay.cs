using SpaceTransit.Ships.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceTransit.Vaulter
{

    [RequireComponent(typeof(RawImage))]
    public sealed class VaulterDisplay : ModuleComponentBase
    {

        private RawImage _image;

        [SerializeField]
        private string group;

        protected override void Awake() => _image = GetComponent<RawImage>();

        protected override void OnInitialized()
        {
            if (Controller.TryGetVaulter(out var controller) && controller.Renderers.TryGetValue(group, out var vaulterRenderer))
                _image.texture = vaulterRenderer.Texture;
            else
                _image.color = Color.black;
        }

    }

}

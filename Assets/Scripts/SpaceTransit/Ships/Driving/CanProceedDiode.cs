using SpaceTransit.Ships.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceTransit.Ships.Driving
{

    [RequireComponent(typeof(Image))]
    public sealed class CanProceedDiode : ModuleComponentBase
    {

        private Image _image;

        protected override void Awake() => _image = GetComponent<Image>();

        private void Update() => _image.color = Controller.CanProceed ? Color.green : Color.red;

    }

}

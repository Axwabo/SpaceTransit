using SpaceTransit.Ships.Modules.Displays;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Driving
{

    public sealed class CanProceedDiode : ModuleUIComponent, ICullingListener
    {

        private VisualElement _element;

        protected override void Initialize(VisualElement root) => _element = root.Q<VisualElement>("Proceed");

        private void Update() => _element.style.backgroundColor = Controller.CanProceed ? Color.green : Color.red;

    }

}

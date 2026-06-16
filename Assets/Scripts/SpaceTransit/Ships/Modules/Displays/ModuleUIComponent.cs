using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Modules.Displays
{

    [RequireComponent(typeof(UIDocument))]
    public abstract class ModuleUIComponent : ModuleComponentBase
    {

        protected override void OnInitialized() => Initialize(this.RootVisual());

        protected abstract void Initialize(VisualElement root);

    }

}

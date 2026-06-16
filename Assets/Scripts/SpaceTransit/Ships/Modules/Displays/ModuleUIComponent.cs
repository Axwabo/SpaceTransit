using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Modules.Displays
{

    [RequireComponent(typeof(UIDocument))]
    public abstract class ModuleUIComponent : ModuleComponentBase
    {

        protected override void OnInitialized()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            Initialize(root);
        }

        protected abstract void Initialize(VisualElement root);

    }

}

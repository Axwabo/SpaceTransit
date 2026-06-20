using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Driving.Screens
{

    [RequireComponent(typeof(ExitListManager))]
    public sealed class ExitList : PickableList<ExitPicker>
    {

        private ExitListManager _manager;

        private bool _loaded;

        private void Awake() => _manager = GetComponent<ExitListManager>();

        protected override void Select(ExitPicker item)
        {
            if (!HasPicked && _manager.State == ShipState.Docked)
                _manager.Assembly.Lock(item);
        }

        protected override string GetContent(ExitPicker item) => item.Exit.Connected.name;

        protected override ListView GetListView(VisualElement root) => root.Q<ListView>("Exits");

    }

}

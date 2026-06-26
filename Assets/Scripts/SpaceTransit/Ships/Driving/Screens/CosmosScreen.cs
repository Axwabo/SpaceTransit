using SpaceTransit.Cosmos;
using SpaceTransit.Loader;
using SpaceTransit.Routes;
using SpaceTransit.Ships.Modules.Displays;
using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Driving.Screens
{

    public sealed class CosmosScreen : ModuleUIComponent, ICullingListener
    {

        public EntryList EntryList { get; private set; }

        public ExitList ExitList { get; private set; }

        private Label _text;

        private StationId _previousStation;

        private Entry _entering;

        private bool _wasInDock;

        private ScreenBase _current;

        private VisualElement _root;

        protected override void Awake()
        {
            base.Awake();
            EntryList = GetComponent<EntryList>();
            ExitList = GetComponent<ExitList>();
        }

        private void OnEnable()
        {
            if (_root == null)
                return;
            _root.SetVisibility(true);
            Update();
        }

        private void OnDisable()
        {
            _previousStation = null;
            _entering = null;
            _wasInDock = false;
            _text.text = "";
            EntryList.Clear();
            ExitList.Clear();
            _root?.SetVisibility(false);
            EntryList.SetVisibility(false);
            ExitList.SetVisibility(false);
        }

        private void Update()
        {
            var tube = Assembly.FrontModule.Thruster.Tube;
            if (tube.TryGetEntryEnsurer(Assembly.Reverse, out var ensurer))
                UpdateEntries(ensurer);
            else if (!_entering || !_entering.IsUsedOnlyBy(Assembly))
                ClearEntryList();
            var inDock = tube is Dock;
            if (_wasInDock && !inDock)
            {
                ExitList.Clear();
                ExitList.SetVisibility(false);
                ExitList.Text = "";
                if (EntryList.Source.Count == 0)
                    _text.text = "";
            }

            if (!_wasInDock && inDock)
                _previousStation = null;

            _wasInDock = inDock;
        }

        protected override void Initialize(VisualElement root)
        {
            _root = root;
            _text = root.Q<Label>("Station");
            EntryList.Initialize();
            ExitList.Initialize();
            EntryList.SetVisibility(false);
            ExitList.SetVisibility(false);
            OnStateChanged();
        }

        public void Select(int index)
        {
            if (!_current)
                return;
            _current.Select(index);
            _current.Confirm();
        }

        public override void OnStateChanged()
        {
            if (State != ShipState.Docked || !isActiveAndEnabled)
                return;
            _previousStation = null;
            if (Assembly.FrontModule.Thruster.Tube is not Dock dock)
                return;
            ClearEntryList();
            UpdateExits(dock);
        }

        private void ClearEntryList()
        {
            EntryList.Clear();
            EntryList.SetVisibility(false);
            EntryList.Text = "";
            _entering = null;
        }

        private void UpdateEntries(IEntryEnsurer ensurer)
        {
            if (LoadingProgress.Current != null || ensurer.Entries.Count == 0)
                return;
            var station = ensurer.TargetStation;
            if (_previousStation == station)
                return;
            _text.text = station.name;
            _previousStation = station;
            EntryList.Source.Clear();
            foreach (var entry in ensurer.Entries)
                EntryList.Source.Add(new EntryPicker(entry));
            EntryList.Source.Sort((a, b) => a.Entry.Dock.Index - b.Entry.Dock.Index);
            EntryList.Refresh();
            EntryList.SetVisibility(true);
            EntryList.Text = "Enter Dock";
            _current = EntryList;
        }

        private void UpdateExits(Dock dock)
        {
            if (ExitList.HasPicked)
                return;
            ExitList.Source.Clear();
            foreach (var exit in dock.FrontExits)
                ExitList.Source.Add(new ExitPicker(exit));
            foreach (var exit in dock.BackExits)
                ExitList.Source.Add(new ExitPicker(exit));
            if (ExitList.Source.Count == 0)
                return;
            ExitList.Refresh();
            ExitList.SetVisibility(true);
            ExitList.Text = "Exit Towards";
            _text.text = dock.Station.Name;
            _current = ExitList;
        }

        public void OnEntrySelected(Entry entry)
        {
            if (!isActiveAndEnabled)
                return;
            _entering = entry;
            UpdateExits(entry.Dock);
        }

    }

}

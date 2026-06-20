using SpaceTransit.Cosmos;
using SpaceTransit.Loader;
using SpaceTransit.Routes;
using SpaceTransit.Ships.Modules.Displays;
using UnityEngine.UIElements;

namespace SpaceTransit.Ships.Driving.Screens
{

    public sealed class CosmosScreen : ModuleUIComponent, ICullingListener
    {

        private EntryList _entryList;

        private ExitList _exitList;

        private Label _text;

        private StationId _previousStation;

        private ScreenBase _current;

        private VisualElement _root;

        protected override void Awake()
        {
            base.Awake();
            _entryList = GetComponent<EntryList>();
            _exitList = GetComponent<ExitList>();
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
            _root?.SetVisibility(false);
            _entryList.SetVisibility(false);
            _exitList.SetVisibility(false);
        }

        private void Update()
        {
            var tube = Assembly.FrontModule.Thruster.Tube;
            if (tube.TryGetEntryEnsurer(Assembly.Reverse, out var ensurer))
                UpdateStation(ensurer);
            else if (_entryList.Source.Count != 0)
            {
                _entryList.Clear();
                _entryList.SetVisibility(false);
            }
        }

        protected override void Initialize(VisualElement root)
        {
            _root = root;
            _text = root.Q<Label>("Station");
            _entryList.Initialize();
            _exitList.Initialize();
        }

        public void Select(int index)
        {
            if (!_current)
                return;
            _current.Select(index);
            _current.Confirm();
        }

        public bool SelectDock(int dockIndex) => _entryList.CanPick && isActiveAndEnabled && _entryList.Select(dockIndex);

        public override void OnStateChanged()
        {
            if (State == ShipState.Docked)
            {
                _previousStation = null;
            }
        }

        private void UpdateStation(EntryEnsurer ensurer)
        {
            if (LoadingProgress.Current != null || ensurer.Entries.Count == 0)
                return;
            var station = ensurer.station;
            if (_previousStation == station)
                return;
            _text.text = station.name;
            _previousStation = station;
            _entryList.Source.Clear();
            foreach (var entry in ensurer.Entries)
                _entryList.Source.Add(new EntryPicker(entry));
            _entryList.Source.Sort((a, b) => a.Entry.Dock.Index - b.Entry.Dock.Index);
            _entryList.Refresh();
            _entryList.Text = "Enter Dock";
            _entryList.SetVisibility(true);
            _current = _entryList;
        }

    }

}

using System;
using System.IO;
using System.Linq;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Stations.Announcements.Katilects;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SpaceTransit.Routes
{

    [CreateAssetMenu(fileName = "Route", menuName = "SpaceTransit/Route", order = 0)]
    public sealed class RouteDescriptor : ScriptableObject
    {

        private IntermediateStop[] _intermediateStops;

        private Passthrough[] _passthrough;

        [field: SerializeField]
        public ServiceType Type { get; private set; }

        [field: SerializeField]
        public bool EveryStation { get; private set; }

        [field: SerializeField]
        public bool Reverse { get; private set; }

        [field: SerializeField]
        public Origin Origin { get; private set; }

        [SerializeField]
        private IntermediateStop[] intermediateStops;

        [field: SerializeField]
        public Destination Destination { get; private set; }

        [SerializeField]
        private RelativeSchedule schedule;

        [SerializeField]
        private Passthrough[] passthrough;

        public ReadOnlySpan<IntermediateStop> IntermediateStops => _intermediateStops;

        public ReadOnlySpan<Passthrough> Passthrough => _passthrough;

        public IKatilect Katilect => schedule ? schedule.katilectOverride : null;

        private void Awake()
        {
            var hasSchedule = schedule != null;
            _intermediateStops = hasSchedule
                ? schedule.intermediateStops.Select(e => e.Add(Origin.Departure.Value)).ToArray()
                : intermediateStops;
            _passthrough = hasSchedule ? schedule.passthrough : passthrough;
        }

#if UNITY_EDITOR
        private void OnValidate() => Awake();

        [ContextMenu("Create Schedule")]
        private void CreateSchedule()
        {
            var path = EditorUtility.SaveFilePanel("Save Relative Schedule", null, name, "asset");
            if (string.IsNullOrEmpty(path))
                return;
            var relativeSchedule = CreateInstance<RelativeSchedule>();
            relativeSchedule.intermediateStops = intermediateStops.Select(e => e.Add(-Origin.Departure.Value)).ToArray();
            relativeSchedule.passthrough = passthrough;
            schedule = relativeSchedule;
            EditorUtility.SetDirty(this);
            AssetDatabase.CreateAsset(relativeSchedule, Path.Combine("Assets", Path.GetRelativePath(Application.dataPath, path)));
        }

        [ContextMenu("Make Absolute")]
        private void MakeAbsolute()
        {
            if (!schedule)
                return;
            Awake();
            intermediateStops = _intermediateStops;
            passthrough = _passthrough;
            schedule = null;
            Awake();
            EditorUtility.SetDirty(this);
        }
#endif

    }

}

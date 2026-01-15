using System;
using System.IO;
using System.Linq;
using SpaceTransit.Routes.Stops;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace SpaceTransit.Routes
{

    [CreateAssetMenu(fileName = "Route", menuName = "SpaceTransit/Route", order = 0)]
    public sealed class RouteDescriptor : ScriptableObject
    {

        private IntermediateStop[] _intermediateStops;

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

        [SerializeField]
        private RelativeSchedule schedule;

        public ReadOnlySpan<IntermediateStop> IntermediateStops => _intermediateStops;

        [field: SerializeField]
        public Destination Destination { get; private set; }

        private void Awake() => _intermediateStops = schedule
            ? schedule.intermediateStops.Select(e => e.Absolute(Origin.Departure.Value)).ToArray()
            : intermediateStops;

#if UNITY_EDITOR
        [ContextMenu("Create Schedule")]
        private void CreateSchedule()
        {
            var path = EditorUtility.SaveFilePanel("Save Relative Schedule", null, name, "asset");
            if (string.IsNullOrEmpty(path))
                return;
            var relativeSchedule = CreateInstance<RelativeSchedule>();
            relativeSchedule.intermediateStops = intermediateStops.Select(e => e.Relative(Origin.Departure.Value)).ToArray();
            schedule = relativeSchedule;
            EditorUtility.SetDirty(this);
            AssetDatabase.CreateAsset(relativeSchedule, Path.Combine("Assets", Path.GetRelativePath(Application.dataPath, path)));
        }
#endif

    }

}

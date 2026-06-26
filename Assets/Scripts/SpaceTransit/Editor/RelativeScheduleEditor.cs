using System;
using SpaceTransit.Routes;
using UnityEditor;
using UnityEngine;

namespace SpaceTransit.Editor
{

    [CustomEditor(typeof(RelativeSchedule))]
    [CanEditMultipleObjects]
    public sealed class RelativeScheduleEditor : UnityEditor.Editor
    {

        private static int _offset;

        public override void OnInspectorGUI()
        {
            _offset = EditorGUILayout.IntField("Offset By", _offset);
            if (GUILayout.Button("Offset"))
                Offset();
            GUILayout.Space(20);
            base.OnInspectorGUI();
        }

        private void Offset()
        {
            var offset = TimeSpan.FromMinutes(_offset);
            foreach (var o in targets)
                Offset((RelativeSchedule) o, offset);
        }

        private static void Offset(RelativeSchedule schedule, TimeSpan offset)
        {
            foreach (var stop in schedule.intermediateStops)
            {
                stop.Arrival = stop.Arrival.Value + offset;
                stop.Departure = stop.Departure.Value + offset;
            }

            EditorUtility.SetDirty(schedule);
        }

    }

}

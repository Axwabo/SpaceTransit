using System;
using UnityEditor;
using UnityEngine;

namespace SpaceTransit.Editor
{

    [CustomPropertyDrawer(typeof(TimeOnly))]
    public class TimeOnlyDrawer : PropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var time = (TimeOnly) property.boxedValue;
            var hours = EditorGUI.IntSlider(position, "Hours", time.Value.Hours, 0, 23);
            var minutes = EditorGUI.IntSlider(position, "Minutes", time.Value.Minutes, 0, 23);
            var seconds = EditorGUI.IntSlider(position, "Seconds", time.Value.Seconds, 0, 23);
            time.Value = new TimeSpan(hours, minutes, seconds);
        }

    }

}

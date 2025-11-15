using System;
using UnityEditor;
using UnityEngine;

namespace SpaceTransit.Editor
{

    [CustomPropertyDrawer(typeof(TimeOnly))]
    public sealed class TimeOnlyDrawer : PropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var time = (TimeOnly) property.boxedValue;
            var value = EditorGUI.TextField(position, label, $"{time.Value:hh':'mm}");
            if (!value.Contains(':'))
                return;
            var split = value.Split(':');
            if (!int.TryParse(split[0], out var h) || !int.TryParse(split[1], out var m))
                return;
            var timeValue = new TimeSpan(h, m, 0);
            if (timeValue != time.Value)
                property.boxedValue = (TimeOnly) timeValue;
        }

    }

}

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
            var value = EditorGUI.TextField(position, label, $"{time.Value:hh':'mm}");
            if (TimeSpan.TryParse(value, out var result))
                time.Value = result;
        }

    }

}

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public static class UIElementExtensions
{

    [InitializeOnLoadMethod]
    private static void Init()
    {
        ConverterGroups.RegisterGlobalConverter<bool, StyleEnum<DisplayStyle>>((ref bool display) => display ? DisplayStyle.Flex : DisplayStyle.None);
        ConverterGroups.RegisterGlobalConverter<Color, StyleColor>((ref Color color) => color);
    }

    public static void SetVisibility(this VisualElement element, bool visible)
        => element.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;

}

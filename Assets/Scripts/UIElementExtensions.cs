using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class UIElementExtensions
{

#if UNITY_EDITOR
    [InitializeOnLoadMethod]
#endif
    [RuntimeInitializeOnLoadMethod]
    private static void Init() => ConverterGroups.RegisterGlobalConverter<bool, StyleEnum<DisplayStyle>>((ref bool display) => display ? DisplayStyle.Flex : DisplayStyle.None);

    public static void SetVisibility(this VisualElement element, bool visible)
        => element.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;

}

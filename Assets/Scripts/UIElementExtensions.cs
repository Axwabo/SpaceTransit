using UnityEditor;
using UnityEngine.UIElements;

public static class UIElementExtensions
{

    [InitializeOnLoadMethod]
    private static void Init() => ConverterGroups.RegisterGlobalConverter((ref bool display) => display ? DisplayStyle.Flex : DisplayStyle.None);

    public static void SetVisibility(this VisualElement element, bool visible)
        => element.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;

}

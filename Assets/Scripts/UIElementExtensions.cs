using UnityEngine.UIElements;

public static class UIElementExtensions
{

    public static void SetVisibility(this VisualElement element, bool visible)
        => element.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;

}

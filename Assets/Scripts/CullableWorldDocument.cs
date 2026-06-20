using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public sealed class CullableWorldDocument : MonoBehaviour, ICullingListener
{

    private VisualElement _root;

    private void OnEnable() => _root?.SetVisibility(true);

    private void OnDisable() => _root?.SetVisibility(false);

    private void Start() => _root = this.RootVisual();

}

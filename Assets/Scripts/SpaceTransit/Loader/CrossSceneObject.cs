using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace SpaceTransit.Loader.References
{

    public sealed class CrossSceneObject : MonoBehaviour
    {

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void Allow() => EditorSceneManager.preventCrossSceneReferences = false;
#endif

        public static Dictionary<string, GameObject> Loaded { get; } = new();

        public static string GetOrCreate(Component component, Component sceneReference)
            => component ? GetOrCreate(component.gameObject, sceneReference) : null;

        public static string GetOrCreate(GameObject gameObject, Component sceneReference)
        {
            if (!gameObject || gameObject.scene == sceneReference.gameObject.scene)
                return null;
            if (!gameObject.TryGetComponent(out CrossSceneObject reference))
                reference = gameObject.AddComponent<CrossSceneObject>();
            if (!string.IsNullOrEmpty(reference.id))
                return reference.id;
            var id = reference.id = GUID.Generate().ToString();
            Loaded[id] = gameObject;
#if UNITY_EDITOR
            EditorUtility.SetDirty(gameObject);
#endif
            return id;
        }

        public static bool TryGetComponent<T>(string id, out T component)
        {
            if (Loaded.TryGetValue(id, out var go) && go.TryGetComponent(out component))
                return true;
            component = default;
            return false;
        }

        public static T GetComponent<T>(string id, T fallback = default) => string.IsNullOrEmpty(id) || !Loaded.TryGetValue(id, out var go)
            ? fallback
            : go.TryGetComponent(out T component)
                ? component
                : throw new MissingComponentException($"Component of type {typeof(T).FullName} was not found on reference {id}");

        [SerializeField]
        [HideInInspector]
        private string id;

        private void Awake()
        {
            if (!string.IsNullOrEmpty(id))
                Loaded[id] = gameObject;
        }

        private void OnDestroy() => Loaded.Remove(id ?? "");

    }

}

using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SpaceTransit.Loader.References
{

    public sealed class CrossSceneObject : MonoBehaviour
    {

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void Allow() => EditorSceneManager.preventCrossSceneReferences = false;
#endif

        public static Dictionary<string, GameObject> Loaded { get; } = new();

        public static string GetOrCreate(Component component) => GetOrCreate(component.gameObject);

        public static string GetOrCreate(GameObject gameObject)
        {
            if (!gameObject.TryGetComponent(out CrossSceneObject reference))
                reference = gameObject.AddComponent<CrossSceneObject>();
            if (!string.IsNullOrEmpty(reference.id))
                return reference.id;
            var id = reference.id = GUID.Generate().ToString();
            Loaded[id] = reference.gameObject;
            return id;
        }

        public static bool TryGetComponent<T>(string id, out T component)
        {
            if (Loaded.TryGetValue(id, out var go) && go.TryGetComponent(out component))
                return true;
            component = default;
            return false;
        }

        public static T GetComponent<T>(string id) => !Loaded.TryGetValue(id, out var go)
            ? throw new MissingReferenceException($"Cross-scene reference with id {id} not found")
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

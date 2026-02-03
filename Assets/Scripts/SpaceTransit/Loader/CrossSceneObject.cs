using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace SpaceTransit.Loader
{

    public sealed class CrossSceneObject : MonoBehaviour
    {

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void Allow() => EditorSceneManager.preventCrossSceneReferences = false;
#endif

        private static readonly Dictionary<string, GameObject> Loaded = new();

        public static event Action ScenesChanged;

        [RuntimeInitializeOnLoadMethod]
        private static void SubscribeToSceneChanges()
        {
            SceneManager.sceneLoaded += (_, _) => ScenesChanged?.Invoke();
            SceneManager.sceneUnloaded += _ => ScenesChanged?.Invoke();
        }

        public static void SubscribeToSceneChanges(Action action, string reference)
        {
            if (!string.IsNullOrEmpty(reference))
                ScenesChanged += action;
        }

        public static void SubscribeToSceneChanges(Action action, string reference1, string reference2)
        {
            if (!string.IsNullOrEmpty(reference1) || !string.IsNullOrEmpty(reference2))
                ScenesChanged += action;
        }

        public static void SubscribeToSceneChanges(Action action, string[] references)
        {
            if (references is {Length: not 0})
                ScenesChanged += action;
        }

        [SerializeField]
        [HideInInspector]
        private string id;

        private void Awake()
        {
            if (!string.IsNullOrEmpty(id))
                Loaded[id] = gameObject;
        }

        private void OnDestroy() => Loaded.Remove(id ?? "");

        public static string GetOrCreate(Component component, GameObject sceneReference, string originalReference)
            => component ? GetOrCreate(component.gameObject, sceneReference.gameObject, originalReference) : originalReference;

        public static string GetOrCreate(GameObject gameObject, GameObject sceneReference, string originalReference)
        {
            if (!gameObject)
                return originalReference;
            if (gameObject.scene == sceneReference.scene)
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

        public static string[] GetOrCreateAll<T>(T[] objects, GameObject sceneReference, string[] originalReferences) where T : Component
        {
            if (objects is not {Length: not 0})
                return Array.Empty<string>();
            var ids = new string[objects.Length];
            for (var i = 0; i < objects.Length; i++)
            {
                var originalReference = originalReferences != null && originalReferences.Length > i
                    ? originalReferences[i]
                    : null;
                ids[i] = GetOrCreate(objects[i], sceneReference, originalReference);
            }

            return ids;
        }

        public static bool TryGetComponent<T>(string id, out T component)
        {
            if (!string.IsNullOrEmpty(id) && Loaded.TryGetValue(id, out var go) && go.TryGetComponent(out component))
                return true;
            component = default;
            return false;
        }

        public static T GetComponent<T>(string id, T fallback = default) => string.IsNullOrEmpty(id) || !Loaded.TryGetValue(id, out var go)
            ? fallback
            : go.TryGetComponent(out T component)
                ? component
                : throw new MissingComponentException($"Component of type {typeof(T).FullName} was not found on reference {id}");

        public static T[] GetAllComponents<T>(string[] ids, T[] current)
        {
            if (ids is not {Length: not 0})
                return Array.Empty<T>();
            var array = new T[ids.Length];
            for (var i = 0; i < ids.Length; i++)
                if (TryGetComponent(ids[i], out T component))
                    array[i] = component;
                else if (current != null && i < current.Length)
                    array[i] = current[i];
            return array;
        }

    }

}

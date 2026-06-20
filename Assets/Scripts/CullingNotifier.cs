using System.Linq;
using UnityEngine;

public sealed class CullingNotifier : MonoBehaviour
{

    [SerializeField]
    private GameObject[] objects;

    private MonoBehaviour[] _notifiableObjects;

    private void Awake() => _notifiableObjects = objects.SelectMany(e => e.GetComponents<ICullingListener>().OfType<MonoBehaviour>()).ToArray();

    private void OnEnable()
    {
        foreach (var notifiable in _notifiableObjects)
            notifiable.enabled = true;
    }

    private void OnDisable()
    {
        foreach (var notifiable in _notifiableObjects)
            notifiable.enabled = false;
    }

}

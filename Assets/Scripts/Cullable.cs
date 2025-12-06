using SpaceTransit.Movement;
using UnityEngine;

public sealed class Cullable : MonoBehaviour
{

    private bool _previouslyActive;

    [SerializeField]
    private GameObject target;

    private void Awake()
    {
        if (!target)
            enabled = false;
    }

    private void Update()
    {
        var activate = Vector3.Distance(MovementController.Current.LastPosition, transform.position) < 10;
        if (_previouslyActive == activate)
            return;
        _previouslyActive = activate;
        target.SetActive(activate);
    }

}

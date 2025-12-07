using SpaceTransit.Movement;
using UnityEngine;

public sealed class Cullable : MonoBehaviour
{

    private bool _previouslyActive;

    private Transform _t;

    [SerializeField]
    private GameObject target;

    private void Awake()
    {
        if (!target)
            enabled = false;
        else
            _previouslyActive = target.activeSelf;
        _t = transform;
    }

    private void Update()
    {
        var activate = Vector3.Distance(MovementController.Current.LastPosition, _t.position) < 20;
        if (_previouslyActive == activate)
            return;
        _previouslyActive = activate;
        target.SetActive(activate);
    }

}

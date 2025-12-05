using UnityEngine;
using UnityEngine.UI;

namespace SpaceTransit.Menu
{

    [RequireComponent(typeof(Button))]
    public sealed class AutoHideGui : MonoBehaviour
    {

        [SerializeField]
        private GameObject target;

        private void Awake() => GetComponent<Button>().onClick.AddListener(Show);

        private void OnDisable() => target.SetActive(false);

        private void Show() => target.SetActive(!target.activeSelf);

    }

}

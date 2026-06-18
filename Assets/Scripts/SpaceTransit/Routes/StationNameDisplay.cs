using TMPro;
using UnityEngine;

namespace SpaceTransit.Routes
{

    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class StationNameDisplay : MonoBehaviour
    {

        private void Awake() => GetComponent<TextMeshProUGUI>().text = GetComponentInParent<Station>().Name;

    }

}

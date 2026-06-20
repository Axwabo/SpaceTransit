using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Routes
{

    public sealed class StationNameDisplay : MonoBehaviour
    {

        private void Start() => this.RootVisual().Q<Label>("Station").text = GetComponentInParent<Station>().Name;

    }

}

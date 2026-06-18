using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Menu
{

    public sealed class AddButton : MonoBehaviour
    {

        [SerializeField]
        private string buttonName;

        [SerializeField]
        private string content;

        private void Start() => this.RootVisual().Q("Buttons").Add(new Button
        {
            name = buttonName,
            text = content
        });

    }

}

using SpaceTransit.Movement;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Menu
{

    public sealed class TouchscreenOnlyElements : MonoBehaviour
    {

        [SerializeField]
        private string[] names;

        private void Start()
        {
            var root = this.RootVisual();
            foreach (var q in names)
                root.Q(q)?.SetVisibility(TouchscreenMode.Enabled);
        }

    }

}

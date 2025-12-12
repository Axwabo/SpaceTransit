using SpaceTransit.Ships;
using TMPro;
using UnityEngine;

namespace SpaceTransit.Menu
{

    [RequireComponent(typeof(RectTransform))]
    public sealed class MapShip : MonoBehaviour
    {

        [SerializeField]
        private TextMeshProUGUI text;

        [SerializeField]
        private RectTransform point;

        private ShipAssembly _assembly;

        private Transform _anchor;

        private RectTransform _this;

        private void Awake() => _this = (RectTransform) transform;

        public void Apply(Transform anchor, ShipAssembly assembly)
        {
            _assembly = assembly;
            _anchor = anchor;
            UpdatePosition();

            if (assembly.Parent.TryGetVaulter(out var vaulter))
                text.text = vaulter.Route?.Type.ToString() ?? "";
        }

        private void Update() => UpdatePosition();

        private void UpdatePosition()
        {
            // TODO: optimize
            var p = _anchor.InverseTransformPoint(_assembly.FrontModule.Transform.position) * World.MetersToWorld;
            _this.anchoredPosition = new Vector2(p.x, p.z);
            _anchor.eulerAngles = new Vector3(0, 0, _assembly.FrontModule.Transform.localEulerAngles.y);
        }

    }

}

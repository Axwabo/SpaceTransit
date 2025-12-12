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

        public float Scale { get; set; } = 1;

        private void Awake() => _this = (RectTransform) transform;

        public void Apply(Transform anchor, ShipAssembly assembly)
        {
            _assembly = assembly;
            _anchor = anchor;
            UpdatePosition();
            if (assembly.Parent.TryGetVaulter(out var vaulter))
                text.text = vaulter.Route?.name ?? "---";
        }

        private void Update() => UpdatePosition();

        private void UpdatePosition()
        {
            _assembly.FrontModule.Transform.GetLocalPositionAndRotation(out var pos, out var rot);
            var position = _anchor.InverseTransformPoint(pos);
            var rotation = rot.eulerAngles.y;
            _this.anchoredPosition = new Vector2(position.x * Scale, position.z * Scale);
            point.eulerAngles = new Vector3(0, 0, _assembly.Reverse ? rotation : -rotation);
        }

    }

}

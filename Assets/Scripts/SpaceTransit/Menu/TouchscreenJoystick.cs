using SpaceTransit.Movement;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTransit.Menu
{

    public sealed class TouchscreenJoystick : MonoBehaviour
    {

        [SerializeField]
        private int minDistance = 20;

        private VisualElement _joystick;

        private bool _active;

        private void Awake()
        {
            if (!TouchscreenMode.Enabled)
                Destroy(this);
        }

        private void Start()
        {
            _joystick = this.RootVisual().Q("Joystick");
            _joystick.RegisterCallback<PointerDownEvent>(evt =>
            {
                _active = true;
                UpdateMovement(evt.localPosition);
            });
            _joystick.RegisterCallback<PointerUpEvent>(_ => Disable());
            _joystick.RegisterCallback<PointerLeaveEvent>(_ => Disable());
            _joystick.RegisterCallback<PointerMoveEvent>(evt => UpdateMovement(evt.localPosition));
        }

        private void Disable()
        {
            _active = false;
            TouchscreenMode.Movement = Vector2.zero;
        }

        private void UpdateMovement(Vector3 position)
        {
            if (!_active)
                return;
            var relative = (Vector2) position - _joystick.localBound.center;
            TouchscreenMode.Movement = relative.sqrMagnitude < minDistance * minDistance ? Vector2.zero : new Vector2(relative.x, -relative.y);
        }

    }

}

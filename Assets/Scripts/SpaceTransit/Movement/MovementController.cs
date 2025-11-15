using SpaceTransit.Menu;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceTransit.Movement
{

    [RequireComponent(typeof(CharacterController))]
    public sealed class MovementController : MonoBehaviour
    {

        public static MovementController Current { get; private set; }

        [SerializeField]
        private float speed;

        [SerializeField]
        private float gravity;

        [SerializeField]
        private float jumpVelocity;

        [SerializeField]
        private Transform cameraTransform;

        private Transform _t;

        private CharacterController _cc;

        private Transform _mount;

        private Vector3 _mountOffset;

        private float _verticalVelocity;

        public Transform Mount
        {
            get => _mount;
            set => _t.parent = _mount = value;
        }

        public Vector3 Position => _t.position;

        private void Awake()
        {
            _t = transform;
            _cc = GetComponent<CharacterController>();
            Current = this;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Start() => World.Current.position -= Position;

        private void Update()
        {
            UpdateLook();

            if (_cc.isGrounded)
                UpdateGrounded();
            else
                _verticalVelocity += gravity * Time.deltaTime;

            var desiredMove = InputSystem.actions["Move"].ReadValue<Vector2>();
            var move = _t.rotation * new Vector3(desiredMove.x, 0, desiredMove.y).normalized;
            move.y = _verticalVelocity;
            if (move == Vector3.zero)
                return;
            var previous = _t.localPosition;
            _cc.Move(Time.deltaTime * speed * 0.1f * move);
            var delta = _t.localPosition - previous;
            if (delta != Vector3.zero)
                World.Current.position -= _t.TransformVector(delta) * World.MetersToWorld;
        }

        private void UpdateLook()
        {
            if (MenuScreen.IsOpen)
                return;
            var look = InputSystem.actions["Look"].ReadValue<Vector2>();
            if (look == Vector2.zero)
                return;
            _t.Rotate(Vector3.up, look.x * 0.1f);
            cameraTransform.Rotate(Vector3.right, look.y * -0.1f);
        }

        private void UpdateGrounded()
        {
            if (InputSystem.actions["Jump"].IsPressed())
                _verticalVelocity = jumpVelocity;
            else if (_verticalVelocity < 0)
                _verticalVelocity = 0;
        }

        private void OnDestroy() => Cursor.lockState = CursorLockMode.None;

    }

}

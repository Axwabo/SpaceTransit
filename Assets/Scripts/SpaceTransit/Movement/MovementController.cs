using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceTransit.Movement
{

    [RequireComponent(typeof(CharacterController))]
    public sealed class MovementController : MonoBehaviour
    {

        public float speed;

        public float gravity;

        public float jumpVelocity;

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

        private void Awake()
        {
            _t = transform;
            _cc = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (_cc.isGrounded)
                UpdateGrounded();
            else
                _verticalVelocity += gravity * Time.deltaTime;

            var desiredMove = InputSystem.actions["Move"].ReadValue<Vector2>();
            var move = _t.rotation * new Vector3(desiredMove.x, 0, desiredMove.y).normalized;
            move.y = _verticalVelocity;
            if (move != Vector3.zero)
                _cc.Move(Time.deltaTime * speed * move);
        }

        private void UpdateGrounded()
        {
            if (InputSystem.actions["Jump"].IsPressed())
                _verticalVelocity = jumpVelocity;
            else if (_verticalVelocity < 0)
                _verticalVelocity = 0;
        }

    }

}

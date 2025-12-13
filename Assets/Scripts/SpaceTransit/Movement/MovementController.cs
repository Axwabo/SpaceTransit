using SpaceTransit.Menu;
using SpaceTransit.Routes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceTransit.Movement
{

    [RequireComponent(typeof(CharacterController))]
    public sealed class MovementController : MonoBehaviour
    {

        public static MovementController Current { get; private set; }

        public static StationId StartingStation { get; set; }

        [SerializeField]
        private float speed;

        [SerializeField]
        private float gravity;

        [SerializeField]
        private float jumpVelocity;

        [SerializeField]
        private Transform cameraTransform;

        [SerializeField]
        private StationId startingStation;

        private Transform _t;

        private CharacterController _cc;

        private Transform _mount;

        private float _verticalVelocity;

        private bool _relocated;

        public bool IsMounted { get; private set; }

        public Transform Mount
        {
            get => _mount;
            set
            {
                IsMounted = value;
                _mount = value;
                _t.parent = IsMounted ? value : World.Current;
            }
        }

        public Vector3 Position => _t.position;

        public Vector3 LastPosition { get; private set; }

        private void Awake()
        {
            _t = transform;
            _cc = GetComponent<CharacterController>();
            Current = this;
            Time.timeScale = 1;
            if (!StartingStation)
                StartingStation = startingStation;
        }

        private void Update()
        {
            if (!_relocated)
            {
                Relocate();
                return;
            }

            UpdateLook();

            if (_cc.isGrounded)
                UpdateGrounded();
            else
                _verticalVelocity += gravity * Clock.Delta;

            var desiredMove = InputSystem.actions["Move"].ReadValue<Vector2>();
            var move = _t.rotation * new Vector3(desiredMove.x, 0, desiredMove.y).normalized;
            move.y = _verticalVelocity;
            if (move == Vector3.zero)
                return;
            _cc.Move(Clock.Delta * speed * 0.1f * move);
            var current = Position;
            LastPosition = current;
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

        private void OnDestroy()
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 1;
        }

        public void Teleport(Vector3 to)
        {
            _t.position = to;
            _verticalVelocity = 0;
            World.Current.position -= LastPosition - to;
        }

        private void Relocate()
        {
            _relocated = true;
            if (!StartingStation || !Station.TryGetLoadedStation(StartingStation, out var station))
                return;
            var spawnpoint = station.Spawnpoint + Vector3.up * 0.2f;
            _t.position = spawnpoint;
            World.Current.position = -spawnpoint;
            StartingStation = null;
        }

    }

}

using UnityEngine;

namespace _Game._Scripts.PlayerSystem
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private bool _isPlayer;
        [SerializeField] private FloatingJoystick _floatingJoystick;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private Transform _visualsTransform;

        private Vector2 m_MovementInput;

        [SerializeField] private float _knockbackDisableDuration = 0.15f;
        private bool m_MovementLocked;


        private void Awake()
        {
            InitializeRigidbody();
        }

        private void Update()
        {
            if (_isPlayer) ReadInput();
        }

        private void FixedUpdate()
        {
            if (m_MovementLocked)
                return;

            Move();
            HandleFacingDirection();
        }


        private void InitializeRigidbody()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void ReadInput()
        {
            if (!_floatingJoystick)
            {
                m_MovementInput = Vector2.zero;
                return;
            }

            m_MovementInput = new Vector2(
                _floatingJoystick.Horizontal,
                _floatingJoystick.Vertical
            );
        }

        private void Move()
        {
            Vector2 targetVelocity = m_MovementInput * _moveSpeed;
            _rigidbody2D.velocity = new Vector2(
                targetVelocity.x,
                targetVelocity.y
            );
        }

        private void HandleFacingDirection()
        {
            if (Mathf.Abs(m_MovementInput.x) < 0.01f)
                return;

            var localScale = _visualsTransform.localScale;
            localScale.x = m_MovementInput.x > 0
                ? Mathf.Abs(localScale.x)
                : -Mathf.Abs(localScale.x);
            _visualsTransform.localScale = localScale;
        }


        public void LockMovementTemporarily()
        {
            if (m_MovementLocked)
                return;

            m_MovementLocked = true;
            Invoke(nameof(UnlockMovement), _knockbackDisableDuration);
        }

        private void UnlockMovement()
        {
            m_MovementLocked = false;
        }

        private void OnValidate()
        {
            if (!_rigidbody2D)
                _rigidbody2D = GetComponent<Rigidbody2D>();
        }
    }
}
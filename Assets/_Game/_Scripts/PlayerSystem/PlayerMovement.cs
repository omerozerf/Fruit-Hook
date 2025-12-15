using UnityEngine;

namespace _Game._Scripts.PlayerSystem
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private Rigidbody2D _rigidbody2D;

        [Header("Visuals")]
        [SerializeField] private Transform _visualsTransform;

        [Header("Knockback")]
        [SerializeField] private float _knockbackDisableDuration = 0.15f;

        private Vector2 m_MovementInput;
        private bool m_MovementLocked;

        
        private void Awake()
        {
            InitializeRigidbody();
        }

        private void FixedUpdate()
        {
            if (m_MovementLocked)
                return;

            ApplyMove();
            HandleFacingDirection();
        }

        private void InitializeRigidbody()
        {
            if (!_rigidbody2D)
                _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void SetMoveInput(Vector2 input)
        {
            m_MovementInput = Vector2.ClampMagnitude(input, 1f);
        }

        private void ApplyMove()
        {
            Vector2 targetVelocity = m_MovementInput * _moveSpeed;
            _rigidbody2D.velocity = targetVelocity;
        }

        private void HandleFacingDirection()
        {
            if (!_visualsTransform)
                return;

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
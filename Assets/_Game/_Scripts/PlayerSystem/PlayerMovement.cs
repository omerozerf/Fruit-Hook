using _Game._Scripts.ScriptableObjects;
using UnityEngine;

namespace _Game._Scripts.PlayerSystem
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Settings")] [SerializeField] private PlayerMovementSettingsSO _settings;

        [Header("Runtime References")] [SerializeField]
        private Rigidbody2D _rigidbody2D;

        [SerializeField] private Transform _visualsTransform;

        private Vector2 m_MovementInput;
        private bool m_MovementLocked;


        private void Awake()
        {
            if (_settings == null)
            {
                Debug.LogError($"{nameof(PlayerMovement)} on '{name}' has no PlayerMovementSettings assigned.");
                enabled = false;
                return;
            }

            InitializeRigidbody();
        }

        private void FixedUpdate()
        {
            if (m_MovementLocked)
                return;

            ApplyMove();
            HandleFacingDirection();
        }

        private void OnValidate()
        {
            if (!_rigidbody2D)
                _rigidbody2D = GetComponent<Rigidbody2D>();
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
            var targetVelocity = m_MovementInput * _settings.MoveSpeed;
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
            Invoke(nameof(UnlockMovement), _settings.KnockbackDisableDuration);
        }

        private void UnlockMovement()
        {
            m_MovementLocked = false;
        }
    }
}
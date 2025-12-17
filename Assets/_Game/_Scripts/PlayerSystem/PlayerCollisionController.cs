using _Game._Scripts.AudioSystem;
using _Game._Scripts.GameEvents;
using _Game._Scripts.Patterns.EventBusPattern;
using _Game._Scripts.ScriptableObjects;
using _Game._Scripts.SwordBubbleSystem;
using _Game._Scripts.SwordOrbitSystem;
using DG.Tweening;
using UnityEngine;

namespace _Game._Scripts.PlayerSystem
{
    public class PlayerCollisionController : MonoBehaviour
    {
        [SerializeField] private bool _isPlayer;

        [Header("Settings")]
        [SerializeField] private PlayerCollisionSettingsSO _settings;

        [Header("References")]
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerVisualController _playerVisualController;
        [SerializeField] private PlayerHealthController _playerHealthController;
        [SerializeField] private SwordOrbitController _swordOrbitController;
        [SerializeField] private Rigidbody2D _rigidbody2D;

        private Tween m_CameraShakeTween;

        
        private void Awake()
        {
            if (!_settings)
            {
                Debug.LogError(
                    $"{nameof(PlayerCollisionController)} on '{name}' has no PlayerCollisionSettings assigned.");
                enabled = false;
                return;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            HandleSwordBubbleCollision(other);
            HandleSwordCollision(other);
        }

        private void OnValidate()
        {
            if (!_playerHealthController) _playerHealthController = GetComponentInParent<PlayerHealthController>();
            if (!_rigidbody2D) _rigidbody2D = GetComponentInParent<Rigidbody2D>();
            if (!_playerMovement) _playerMovement = GetComponentInParent<PlayerMovement>();
            if (!_playerVisualController) _playerVisualController = GetComponentInParent<PlayerVisualController>();
        }


        private void HandleSwordCollision(Collider2D other)
        {
            if (!IsInLayerMask(other.gameObject, _settings.SwordLayerMask)) return;

            _playerHealthController.TakeDamage(1);
            _playerVisualController.PlayDamageFlash();
            ApplyKnockback(other);
            if (_isPlayer) PlayCameraShake();
            AudioService.Instance.PlaySfx(SfxId.DamageHit);
        }

        private void HandleSwordBubbleCollision(Collider2D other)
        {
            if (!IsInLayerMask(other.gameObject, _settings.BubbleSwordLayerMask)) return;
            if (!other.TryGetComponent(out SwordBubbleCollision swordBubbleCollision)) return;

            swordBubbleCollision.GetSwordBubble().PlayPickupToCenter(transform, () =>
            {
                _swordOrbitController.SpawnSword();
                EventBus<SwordBubbleTaken>.Publish(new SwordBubbleTaken
                {
                    transform = swordBubbleCollision.GetSwordBubble().transform
                });
                if (_isPlayer) AudioService.Instance.PlaySfx(SfxId.BubbleSword);
            });
        }

        private bool IsInLayerMask(GameObject obj, LayerMask mask)
        {
            return (mask.value & (1 << obj.layer)) != 0;
        }
        
        private void ApplyKnockback(Collider2D other)
        {
            if (!_rigidbody2D) return;

            Vector2 knockbackDirection = (transform.position - other.transform.position).normalized;
            _rigidbody2D.AddForce(knockbackDirection * _settings.KnockbackForce, ForceMode2D.Impulse);
            _playerMovement.LockMovementTemporarily();
        }

        private void PlayCameraShake()
        {
            var cam = Camera.main;
            if (!cam) return;

            var camTransform = cam.transform;

            m_CameraShakeTween?.Kill();

            m_CameraShakeTween = camTransform
                .DOShakePosition(
                    _settings.ShakeDuration,
                    new Vector3(_settings.ShakeStrength, _settings.ShakeStrength, 0f),
                    20
                )
                .SetUpdate(true);
        }

        
        public SwordOrbitController GetSwordOrbitController()
        {
            return _swordOrbitController;
        }
    }
}
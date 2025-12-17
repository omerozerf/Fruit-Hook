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

        private const float SAME_COLLIDER_DAMAGE_COOLDOWN = 0.5f;
        private const int COLLIDER_COOLDOWN_CACHE_SIZE = 16;

        private int[] m_ColliderIdCache;
        private float[] m_LastDamageTimeCache;
        private int m_CacheWriteIndex;
        
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

            m_ColliderIdCache = new int[COLLIDER_COOLDOWN_CACHE_SIZE];
            m_LastDamageTimeCache = new float[COLLIDER_COOLDOWN_CACHE_SIZE];

            for (int i = 0; i < m_ColliderIdCache.Length; i++)
                m_ColliderIdCache[i] = int.MinValue;
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

            if (!CanTakeDamageFrom(other))
                return;

            _playerHealthController.TakeDamage(1);
            _playerVisualController.PlayDamageFlash();
            ApplyKnockback(other);
            if (_isPlayer) PlayCameraShake();
            AudioService.Instance.PlaySfx(SfxId.DamageHit);
        }

        private bool CanTakeDamageFrom(Collider2D other)
        {
            int id = other.GetInstanceID();
            float now = Time.time;

            for (int i = 0; i < m_ColliderIdCache.Length; i++)
            {
                if (m_ColliderIdCache[i] != id) continue;

                if (now - m_LastDamageTimeCache[i] < SAME_COLLIDER_DAMAGE_COOLDOWN)
                    return false;

                m_LastDamageTimeCache[i] = now;
                return true;
            }

            m_ColliderIdCache[m_CacheWriteIndex] = id;
            m_LastDamageTimeCache[m_CacheWriteIndex] = now;

            m_CacheWriteIndex++;
            if (m_CacheWriteIndex >= m_ColliderIdCache.Length)
                m_CacheWriteIndex = 0;

            return true;
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
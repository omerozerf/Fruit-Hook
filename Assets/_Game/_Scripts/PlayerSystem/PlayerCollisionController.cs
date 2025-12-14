using System;
using _Game._Scripts.SwordBubbleSystem;
using _Game._Scripts.SwordOrbitSystem;
using UnityEngine;
using DG.Tweening;

namespace _Game._Scripts.PlayerSystem
{
    public class PlayerCollisionController : MonoBehaviour
    {
        [SerializeField] private bool _isPlayer;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerVisualController _playerVisualController;
        [SerializeField] private PlayerHealthController _playerHealthController;
        [SerializeField] private SwordBubbleCreator _swordBubbleCreator;
        [SerializeField] private SwordOrbitController _swordOrbitController;
        [SerializeField] private LayerMask _bubbleSwordLayerMask;
        [SerializeField] private LayerMask _swordLayerMask;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private float _knockbackForce = 5f;
        [SerializeField] private float _shakeDuration = 0.3f;
        [SerializeField] private float _shakeStrength = 0.7f;

        private Tween m_CameraShakeTween;


        private void OnTriggerEnter2D(Collider2D other)
        {
            HandleSwordBubbleCollision(other);

            if (!IsInLayerMask(other.gameObject, _swordLayerMask)) return;

            _playerHealthController.TakeDamage(1);
            _playerVisualController.PlayDamageFlash();
            ApplyKnockback(other);
            if (_isPlayer) PlayCameraShake();
        }

        private void OnValidate()
        {
            if (!_playerHealthController) _playerHealthController = GetComponentInParent<PlayerHealthController>();
            if (!_rigidbody2D) _rigidbody2D = GetComponentInParent<Rigidbody2D>();
            if (!_playerMovement) _playerMovement = GetComponentInParent<PlayerMovement>();
            if (!_playerVisualController) _playerVisualController = GetComponentInParent<PlayerVisualController>();
        }


        private void HandleSwordBubbleCollision(Collider2D other)
        {
            if (!IsInLayerMask(other.gameObject, _bubbleSwordLayerMask)) return;
            if (!other.TryGetComponent(out SwordBubbleCollision swordBubbleCollision)) return;

            swordBubbleCollision.GetSwordBubble().PlayPickupToCenter(transform, () =>
            {
                _swordOrbitController.SpawnSword();
                _swordBubbleCreator.Release(swordBubbleCollision.GetSwordBubble().transform);
            });
        }

        private bool IsInLayerMask(GameObject obj, LayerMask mask)
        {
            return (mask.value & (1 << obj.layer)) != 0;
        }


        private void ApplyKnockback(Collider2D other)
        {
            if (_rigidbody2D == null) return;

            Vector2 knockbackDirection = (transform.position - other.transform.position).normalized;
            _rigidbody2D.AddForce(knockbackDirection * _knockbackForce, ForceMode2D.Impulse);
            _playerMovement.LockMovementTemporarily();
        }

        private void PlayCameraShake()
        {
            Camera cam = Camera.main;
            if (!cam) return;

            Transform camTransform = cam.transform;

            m_CameraShakeTween?.Kill();

            m_CameraShakeTween = camTransform
                .DOShakePosition(
                    _shakeDuration,
                    new Vector3(_shakeStrength, _shakeStrength, 0f),
                    vibrato: 20,
                    randomness: 90f,
                    fadeOut: true
                )
                .SetUpdate(true);
        }

        public SwordOrbitController GetSwordOrbitController()
        {
            return _swordOrbitController;
        }
    }
}
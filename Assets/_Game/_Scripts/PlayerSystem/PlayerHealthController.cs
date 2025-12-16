using System;
using _Game._Scripts.GameEvents;
using _Game._Scripts.Patterns.EventBusPattern;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace _Game._Scripts.PlayerSystem
{
    public class PlayerHealthController : MonoBehaviour
    {
        [SerializeField] private bool _isPlayer;
        
        [Header("UI")]
        [SerializeField] private Image _healthBar;
        [SerializeField] private Image _healthWhiteIndicatorBar;

        [Header("Health Settings")]
        [SerializeField] private int _maxHealth = 3;

        [Header("White Indicator Settings")]
        [SerializeField] private float _whiteBarDelay = 0.15f;
        [SerializeField] private float _whiteBarTweenDuration = 0.4f;

        [Header("Colors")]
        [SerializeField] private Color _fullHealthColor = Color.green;
        [SerializeField] private Color _midHealthColor = new(1f, 0.5f, 0f);
        [SerializeField] private Color _lowHealthColor = Color.red;

        [Header("Death Settings")]
        [SerializeField] private Collider2D _collider;
        [SerializeField] private float _deathMoveDistance = 1.2f;
        [SerializeField] private float _deathDuration = 0.4f;
        [SerializeField] private Ease _deathEase = Ease.InQuad;

        public event Action OnDied;

        private int m_CurrentHealth;
        private bool m_IsDead;
        private Tween m_WhiteBarTween;
        

        private void Awake()
        {
            InitializeHealth();
            if (!_collider)
                _collider = GetComponent<Collider2D>();
        }
        
        private void InitializeHealth()
        {
            m_CurrentHealth = _maxHealth;
            m_IsDead = m_CurrentHealth <= 0;

            float fill = CalculateFillAmount();
            SetHealthBarImmediate(fill);
            SetWhiteBarImmediate(fill);

            UpdateHealthColor();
        }

        public void TakeDamage(int amount)
        {
            if (m_IsDead)
                return;

            if (amount <= 0)
                return;

            SetHealth(m_CurrentHealth - amount);
        }

        private void SetHealth(int newHealth)
        {
            newHealth = Mathf.Clamp(newHealth, 0, _maxHealth);

            if (newHealth == m_CurrentHealth)
                return;

            m_CurrentHealth = newHealth;

            float targetFill = CalculateFillAmount();

            SetHealthBarImmediate(targetFill);
            AnimateWhiteIndicatorTo(targetFill);
            UpdateHealthColor();

            UpdateDeadState();
        }

        public bool IsDead()
        {
            return m_IsDead;
        }

        private void UpdateDeadState()
        {
            if (m_IsDead)
                return;

            if (m_CurrentHealth > 0)
                return;

            m_IsDead = true;
            HandleDeath();
        }

        private void HandleDeath()
        {
            if (_collider)
                _collider.enabled = false;

            EventBus<PlayerDiedEvent>.Publish(new PlayerDiedEvent
            {
                isPlayer = _isPlayer
            });
            
            Vector3 startPos = transform.position;
            Vector3 targetPos = startPos + Vector3.down * _deathMoveDistance;

            Sequence deathSequence = DOTween.Sequence();
            deathSequence
                .Append(transform.DOMoveY(targetPos.y, _deathDuration).SetEase(_deathEase))
                .Join(transform.DOScale(Vector3.zero, _deathDuration).SetEase(_deathEase))
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
        }

        private float CalculateFillAmount()
        {
            return (float)m_CurrentHealth / _maxHealth;
        }

        private void SetHealthBarImmediate(float fill)
        {
            fill = Mathf.Clamp01(fill);

            if (_healthBar)
            {
                Vector3 scale = _healthBar.transform.localScale;
                scale.x = fill;
                _healthBar.transform.localScale = scale;
            }
        }

        private void SetWhiteBarImmediate(float fill)
        {
            fill = Mathf.Clamp01(fill);

            if (_healthWhiteIndicatorBar)
            {
                Vector3 scale = _healthWhiteIndicatorBar.transform.localScale;
                scale.x = fill;
                _healthWhiteIndicatorBar.transform.localScale = scale;
            }
        }

        private void AnimateWhiteIndicatorTo(float targetFill)
        {
            m_WhiteBarTween?.Kill();

            if (_healthWhiteIndicatorBar && _healthWhiteIndicatorBar.transform.localScale.x <= targetFill)
            {
                SetWhiteBarImmediate(targetFill);
                return;
            }

            m_WhiteBarTween = DOVirtual.DelayedCall(_whiteBarDelay, () =>
            {
                m_WhiteBarTween = DOTween.To(
                        () => _healthWhiteIndicatorBar ? _healthWhiteIndicatorBar.transform.localScale.x : targetFill,
                        SetWhiteBarImmediate,
                        targetFill,
                        _whiteBarTweenDuration)
                    .SetEase(Ease.OutQuad);
            });
        }

        private void UpdateHealthColor()
        {
            _healthBar.color = m_CurrentHealth switch
            {
                >= 3 => _fullHealthColor,
                2 => _midHealthColor,
                _ => _lowHealthColor
            };
        }
    }
}
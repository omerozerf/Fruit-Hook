using System;
using _Game._Scripts.GameEvents;
using _Game._Scripts.Patterns.EventBusPattern;
using _Game._Scripts.ScriptableObjects;
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

        [Header("Settings")]
        [SerializeField] private PlayerHealthSettingsSO _settings;

        [Header("Death Settings")]
        [SerializeField] private Collider2D _collider;

        public event Action OnDied;

        private int m_CurrentHealth;
        private bool m_IsDead;
        private Tween m_WhiteBarTween;
        

        private void Awake()
        {
            if (_settings == null)
            {
                Debug.LogError($"{nameof(PlayerHealthController)} on '{name}' has no PlayerHealthSettings assigned.");
                enabled = false;
                return;
            }

            InitializeHealth();

            if (!_collider)
                _collider = GetComponent<Collider2D>();
        }
        
        private void InitializeHealth()
        {
            m_CurrentHealth = _settings.MaxHealth;
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
            newHealth = Mathf.Clamp(newHealth, 0, _settings.MaxHealth);

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
            Vector3 targetPos = startPos + Vector3.down * _settings.DeathMoveDistance;

            Sequence deathSequence = DOTween.Sequence();
            deathSequence
                .Append(transform.DOMoveY(targetPos.y, _settings.DeathDuration).SetEase(_settings.DeathEase))
                .Join(transform.DOScale(Vector3.zero, _settings.DeathDuration).SetEase(_settings.DeathEase))
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
        }

        private float CalculateFillAmount()
        {
            return (float)m_CurrentHealth / _settings.MaxHealth;
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

            m_WhiteBarTween = DOVirtual.DelayedCall(_settings.WhiteBarDelay, () =>
            {
                m_WhiteBarTween = DOTween.To(
                        () => _healthWhiteIndicatorBar ? _healthWhiteIndicatorBar.transform.localScale.x : targetFill,
                        SetWhiteBarImmediate,
                        targetFill,
                        _settings.WhiteBarTweenDuration)
                    .SetEase(Ease.OutQuad);
            });
        }

        private void UpdateHealthColor()
        {
            if (!_healthBar) return;

            _healthBar.color = m_CurrentHealth switch
            {
                >= 3 => _settings.FullHealthColor,
                2 => _settings.MidHealthColor,
                _ => _settings.LowHealthColor
            };
        }
    }
}
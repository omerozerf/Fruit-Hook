using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace _Game._Scripts.PlayerSystem
{
    public class PlayerHealthController : MonoBehaviour
    {
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

        private int m_CurrentHealth;
        private Tween m_WhiteBarTween;

        
        private void Awake()
        {
            InitializeHealth();
        }
        

        private void InitializeHealth()
        {
            m_CurrentHealth = _maxHealth;

            float fill = CalculateFillAmount();
            SetHealthBarImmediate(fill);
            SetWhiteBarImmediate(fill);
            UpdateHealthColor();
        }

        public void TakeDamage(int amount)
        {
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
        }

        private float CalculateFillAmount()
        {
            return (float)m_CurrentHealth / _maxHealth;
        }

        private void SetHealthBarImmediate(float fill)
        {
            _healthBar.fillAmount = fill;
        }

        private void SetWhiteBarImmediate(float fill)
        {
            _healthWhiteIndicatorBar.fillAmount = fill;
        }

        private void AnimateWhiteIndicatorTo(float targetFill)
        {
            m_WhiteBarTween?.Kill();

            if (_healthWhiteIndicatorBar.fillAmount <= targetFill)
            {
                _healthWhiteIndicatorBar.fillAmount = targetFill;
                return;
            }

            m_WhiteBarTween = DOVirtual.DelayedCall(_whiteBarDelay, () =>
            {
                m_WhiteBarTween = _healthWhiteIndicatorBar
                    .DOFillAmount(targetFill, _whiteBarTweenDuration)
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
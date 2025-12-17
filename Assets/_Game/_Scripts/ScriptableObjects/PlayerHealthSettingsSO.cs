using DG.Tweening;
using UnityEngine;

namespace _Game._Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "PlayerHealthSettingsSO",
        menuName = "Game/Player/Health Settings",
        order = 0)]
    public class PlayerHealthSettingsSO : ScriptableObject
    {
        [Header("Health")]
        [SerializeField] private int _maxHealth;

        [Header("White Indicator")]
        [SerializeField] private float _whiteBarDelay;
        [SerializeField] private float _whiteBarTweenDuration;

        [Header("Colors")]
        [SerializeField] private Color _fullHealthColor = Color.green;
        [SerializeField] private Color _midHealthColor = new(1f, 0.5f, 0f);
        [SerializeField] private Color _lowHealthColor = Color.red;

        [Header("Death")]
        [SerializeField] private float _deathMoveDistance;
        [SerializeField] private float _deathDuration;
        [SerializeField] private Ease _deathEase = Ease.InQuad;

        
        public int MaxHealth => _maxHealth;
        public float WhiteBarDelay => _whiteBarDelay;
        public float WhiteBarTweenDuration => _whiteBarTweenDuration;
        public Color FullHealthColor => _fullHealthColor;
        public Color MidHealthColor => _midHealthColor;
        public Color LowHealthColor => _lowHealthColor;
        public float DeathMoveDistance => _deathMoveDistance;
        public float DeathDuration => _deathDuration;
        public Ease DeathEase => _deathEase;
    }
}
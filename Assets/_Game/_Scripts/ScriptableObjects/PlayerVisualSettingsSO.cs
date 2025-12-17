using UnityEngine;

namespace _Game._Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "PlayerVisualSettingsSO",
        menuName = "Game/Player/Visual Settings",
        order = 0)]
    public class PlayerVisualSettingsSO : ScriptableObject
    {
        [Header("Damage Flash")]
        [SerializeField] private float _damageFlashDuration;
        public float DamageFlashDuration => _damageFlashDuration;
    }
}
using UnityEngine;

namespace _Game._Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "PlayerMovementSettingsSO",
        menuName = "Game/Player/Movement Settings",
        order = 0)]
    public class PlayerMovementSettingsSO : ScriptableObject
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed;

        [Header("Knockback")]
        [SerializeField] private float _knockbackDisableDuration;

        public float MoveSpeed => _moveSpeed;
        public float KnockbackDisableDuration => _knockbackDisableDuration;
    }
}
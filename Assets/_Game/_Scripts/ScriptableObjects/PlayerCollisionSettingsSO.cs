using UnityEngine;

namespace _Game._Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "CommonCollisionSettings",
        menuName = "Game/Player/Collision Settings",
        order = 0)]
    public class PlayerCollisionSettingsSO : ScriptableObject
    {
        [Header("Layers")] [SerializeField] private LayerMask _bubbleSwordLayerMask;

        [SerializeField] private LayerMask _swordLayerMask;

        [Header("Knockback")] [SerializeField] private float _knockbackForce = 5f;

        [Header("Camera Shake")] [SerializeField]
        private float _shakeDuration = 0.3f;

        [SerializeField] private float _shakeStrength = 0.7f;

        public LayerMask BubbleSwordLayerMask => _bubbleSwordLayerMask;
        public LayerMask SwordLayerMask => _swordLayerMask;

        public float KnockbackForce => _knockbackForce;

        public float ShakeDuration => _shakeDuration;
        public float ShakeStrength => _shakeStrength;
    }
}
using UnityEngine;

namespace _Game._Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "SwordBubbleSettings",
        menuName = "Game/Sword Bubble/Bubble Settings",
        order = 0)]
    public class SwordBubbleSettingsSO : ScriptableObject
    {
        [Header("Sword Visual")]
        [SerializeField] private float _rotationSpeed = 180f;

        [Header("Pickup Defaults")]
        [SerializeField] private float _defaultPushDistance = 0.35f;
        [SerializeField] private float _defaultPushDuration = 0.15f;
        [SerializeField] private float _defaultHoldDuration = 0.06f;
        [SerializeField] private float _defaultPullDuration = 0.28f;
        [SerializeField] private float _defaultEndScaleMultiplier = 0.15f;
        [SerializeField] private bool _disableColliderOnPickup = true;

        [Header("Easing")]
        [SerializeField] private AnimationCurve _moveEase;
        [SerializeField] private AnimationCurve _scaleEase;


        public float RotationSpeed => _rotationSpeed;
        public float DefaultPushDistance => _defaultPushDistance;
        public float DefaultPushDuration => _defaultPushDuration;
        public float DefaultHoldDuration => _defaultHoldDuration;
        public float DefaultPullDuration => _defaultPullDuration;
        public float DefaultEndScaleMultiplier => _defaultEndScaleMultiplier;
        public bool DisableColliderOnPickup => _disableColliderOnPickup;


        public AnimationCurve MoveEase => _moveEase;
        public AnimationCurve ScaleEase => _scaleEase;
    }
}
using UnityEngine;

namespace _Game._Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "PlayerAnimatorSettingsSO",
        menuName = "Game/Player/Animator Settings",
        order = 0)]
    public class PlayerAnimatorSettingsSO : ScriptableObject
    {
        [Header("Idle")]
        [SerializeField] private float _idleMoveY;
        [SerializeField] private float _idleMoveX;
        [SerializeField] private float _idleDuration;
        [SerializeField] private float _movingIdleSpeedMultiplier;

        [Header("Feet Step")]
        [SerializeField] private float _footStepAngle;
        [SerializeField] private float _footStepDuration;
        [SerializeField] private float _footStepMoveX;


        public float IdleMoveY => _idleMoveY;
        public float IdleMoveX => _idleMoveX;
        public float IdleDuration => _idleDuration;
        public float MovingIdleSpeedMultiplier => _movingIdleSpeedMultiplier;
        public float FootStepAngle => _footStepAngle;
        public float FootStepDuration => _footStepDuration;
        public float FootStepMoveX => _footStepMoveX;
    }
}
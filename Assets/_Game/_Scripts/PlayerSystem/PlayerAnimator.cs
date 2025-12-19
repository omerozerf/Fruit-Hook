using _Game._Scripts.ScriptableObjects;
using DG.Tweening;
using UnityEngine;

namespace _Game._Scripts.PlayerSystem
{
    public class PlayerAnimator : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private PlayerAnimatorSettingsSO _settings;

        [Header("References")]
        [SerializeField] private Transform _bodyTransform;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private Transform _leftFootTransform;
        [SerializeField] private Transform _rightFootTransform;

        private Sequence m_FeetStepSequence;
        private Tween m_IdleTween;
        private bool m_IsMoving;
        private Vector3 m_LeftFootStartLocalEuler;
        private Vector3 m_LeftFootStartLocalPos;
        private Vector3 m_RightFootStartLocalEuler;
        private Vector3 m_RightFootStartLocalPos;


        private void Awake()
        {
            if (!_settings)
            {
                Debug.LogError($"{nameof(PlayerAnimator)} on '{name}' has no PlayerAnimatorSettings assigned.");
                enabled = false;
            }
        }

        private void Start()
        {
            CacheFootDefaults();
            StartIdleAnimation();
        }

        private void Update()
        {
            UpdateIdleSpeedByMovement();
            UpdateFeetAnimationByMovement();
        }

        private void OnDisable()
        {
            StopIdleAnimation();
            StopFeetStepAnimation();
        }

        private void OnValidate()
        {
            if (!_rigidbody2D)
                _rigidbody2D = GetComponent<Rigidbody2D>();
        }


        private void CacheFootDefaults()
        {
            if (_leftFootTransform)
            {
                m_LeftFootStartLocalPos = _leftFootTransform.localPosition;
                m_LeftFootStartLocalEuler = _leftFootTransform.localEulerAngles;
            }

            if (_rightFootTransform)
            {
                m_RightFootStartLocalPos = _rightFootTransform.localPosition;
                m_RightFootStartLocalEuler = _rightFootTransform.localEulerAngles;
            }
        }

        private void UpdateFeetAnimationByMovement()
        {
            if (!_rigidbody2D)
                return;

            var isMovingNow = _rigidbody2D.velocity.sqrMagnitude > 0.01f;

            if (isMovingNow == m_IsMoving)
                return;

            m_IsMoving = isMovingNow;

            if (m_IsMoving)
                StartFeetStepAnimation();
            else
                StopFeetStepAnimation();
        }

        private void StartFeetStepAnimation()
        {
            if (!_leftFootTransform || !_rightFootTransform)
                return;

            StopFeetStepAnimation();

            m_FeetStepSequence = DOTween.Sequence();

            // Phase A
            m_FeetStepSequence.Append(_leftFootTransform
                .DOLocalMoveX(m_LeftFootStartLocalPos.x + _settings.FootStepMoveX, _settings.FootStepDuration)
                .SetEase(Ease.InOutSine));
            m_FeetStepSequence.Join(_rightFootTransform
                .DOLocalMoveX(m_RightFootStartLocalPos.x - _settings.FootStepMoveX, _settings.FootStepDuration)
                .SetEase(Ease.InOutSine));

            m_FeetStepSequence.Join(_leftFootTransform
                .DOLocalRotate(
                    new Vector3(
                        m_LeftFootStartLocalEuler.x,
                        m_LeftFootStartLocalEuler.y,
                        m_LeftFootStartLocalEuler.z - _settings.FootStepAngle),
                    _settings.FootStepDuration)
                .SetEase(Ease.InOutSine));
            m_FeetStepSequence.Join(_rightFootTransform
                .DOLocalRotate(
                    new Vector3(
                        m_RightFootStartLocalEuler.x,
                        m_RightFootStartLocalEuler.y,
                        m_RightFootStartLocalEuler.z + _settings.FootStepAngle),
                    _settings.FootStepDuration)
                .SetEase(Ease.InOutSine));

            // Phase B (returns naturally, no snap)
            m_FeetStepSequence.Append(_leftFootTransform
                .DOLocalMoveX(m_LeftFootStartLocalPos.x, _settings.FootStepDuration)
                .SetEase(Ease.InOutSine));
            m_FeetStepSequence.Join(_rightFootTransform
                .DOLocalMoveX(m_RightFootStartLocalPos.x, _settings.FootStepDuration)
                .SetEase(Ease.InOutSine));

            m_FeetStepSequence.Join(_leftFootTransform
                .DOLocalRotate(
                    new Vector3(
                        m_LeftFootStartLocalEuler.x,
                        m_LeftFootStartLocalEuler.y,
                        m_LeftFootStartLocalEuler.z),
                    _settings.FootStepDuration)
                .SetEase(Ease.InOutSine));
            m_FeetStepSequence.Join(_rightFootTransform
                .DOLocalRotate(
                    new Vector3(
                        m_RightFootStartLocalEuler.x,
                        m_RightFootStartLocalEuler.y,
                        m_RightFootStartLocalEuler.z),
                    _settings.FootStepDuration)
                .SetEase(Ease.InOutSine));

            m_FeetStepSequence.SetLoops(-1, LoopType.Restart);
        }

        private void StopFeetStepAnimation()
        {
            if (m_FeetStepSequence != null && m_FeetStepSequence.IsActive())
            {
                m_FeetStepSequence.Kill();
                m_FeetStepSequence = null;
            }

            if (_leftFootTransform)
            {
                _leftFootTransform.localPosition = m_LeftFootStartLocalPos;
                _leftFootTransform.localEulerAngles = m_LeftFootStartLocalEuler;
            }

            if (_rightFootTransform)
            {
                _rightFootTransform.localPosition = m_RightFootStartLocalPos;
                _rightFootTransform.localEulerAngles = m_RightFootStartLocalEuler;
            }
        }

        private void UpdateIdleSpeedByMovement()
        {
            if (m_IdleTween == null || !_rigidbody2D)
                return;

            var speed = _rigidbody2D.velocity.sqrMagnitude > 0.01f
                ? _settings.MovingIdleSpeedMultiplier
                : 1f;

            m_IdleTween.timeScale = speed;
        }

        private void StartIdleAnimation()
        {
            if (!_bodyTransform)
                return;

            StopIdleAnimation();

            m_IdleTween = _bodyTransform
                .DOScale(new Vector3(_settings.IdleMoveX, _settings.IdleMoveY, 1f), _settings.IdleDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void StopIdleAnimation()
        {
            if (m_IdleTween != null && m_IdleTween.IsActive())
            {
                m_IdleTween.Kill();
                m_IdleTween = null;
            }
        }
    }
}
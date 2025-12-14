using DG.Tweening;
using UnityEngine;

namespace _Game._Scripts.PlayerSystem
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Transform _bodyTransform;
        [SerializeField] private float _idleMoveY;
        [SerializeField] private float _idleMoveX;
        [SerializeField] private float _idleDuration = 0.8f;
        [SerializeField] private float _movingIdleSpeedMultiplier = 1.5f;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private Transform _leftFootTransform;
        [SerializeField] private Transform _rightFootTransform;

        [SerializeField] private float _footStepAngle = 12f;
        [SerializeField] private float _footStepDuration = 0.12f;
        [SerializeField] private float _footStepMoveX = 0.03f;

        private Tween m_IdleTween;
        private Sequence m_FeetStepSequence;
        private bool m_IsMoving;
        private Vector3 m_LeftFootStartLocalPos;
        private Vector3 m_RightFootStartLocalPos;
        private Vector3 m_LeftFootStartLocalEuler;
        private Vector3 m_RightFootStartLocalEuler;

        private void Start()
        {
            CacheFootDefaults();
            StartIdleAnimation();
        }

        private void OnValidate()
        {
            if (!_rigidbody2D) _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void OnDisable()
        {
            StopIdleAnimation();
            StopFeetStepAnimation();
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

            bool isMovingNow = _rigidbody2D.velocity.sqrMagnitude > 0.01f;

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
            if (_leftFootTransform == null || _rightFootTransform == null)
                return;

            StopFeetStepAnimation();

            m_FeetStepSequence = DOTween.Sequence();

            // Phase A
            m_FeetStepSequence.Append(_leftFootTransform
                .DOLocalMoveX(m_LeftFootStartLocalPos.x + _footStepMoveX, _footStepDuration)
                .SetEase(Ease.InOutSine));
            m_FeetStepSequence.Join(_rightFootTransform
                .DOLocalMoveX(m_RightFootStartLocalPos.x - _footStepMoveX, _footStepDuration)
                .SetEase(Ease.InOutSine));

            m_FeetStepSequence.Join(_leftFootTransform
                .DOLocalRotate(
                    new Vector3(
                        m_LeftFootStartLocalEuler.x,
                        m_LeftFootStartLocalEuler.y,
                        m_LeftFootStartLocalEuler.z - _footStepAngle),
                    _footStepDuration)
                .SetEase(Ease.InOutSine));
            m_FeetStepSequence.Join(_rightFootTransform
                .DOLocalRotate(
                    new Vector3(
                        m_RightFootStartLocalEuler.x,
                        m_RightFootStartLocalEuler.y,
                        m_RightFootStartLocalEuler.z + _footStepAngle),
                    _footStepDuration)
                .SetEase(Ease.InOutSine));

            // Phase B (returns naturally, no snap)
            m_FeetStepSequence.Append(_leftFootTransform
                .DOLocalMoveX(m_LeftFootStartLocalPos.x, _footStepDuration)
                .SetEase(Ease.InOutSine));
            m_FeetStepSequence.Join(_rightFootTransform
                .DOLocalMoveX(m_RightFootStartLocalPos.x, _footStepDuration)
                .SetEase(Ease.InOutSine));

            m_FeetStepSequence.Join(_leftFootTransform
                .DOLocalRotate(
                    new Vector3(
                        m_LeftFootStartLocalEuler.x,
                        m_LeftFootStartLocalEuler.y,
                        m_LeftFootStartLocalEuler.z),
                    _footStepDuration)
                .SetEase(Ease.InOutSine));
            m_FeetStepSequence.Join(_rightFootTransform
                .DOLocalRotate(
                    new Vector3(
                        m_RightFootStartLocalEuler.x,
                        m_RightFootStartLocalEuler.y,
                        m_RightFootStartLocalEuler.z),
                    _footStepDuration)
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

            // Reset to cached defaults when stopping.
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

            float speed = _rigidbody2D.velocity.sqrMagnitude > 0.01f
                ? _movingIdleSpeedMultiplier
                : 1f;

            m_IdleTween.timeScale = speed;
        }

        private void Update()
        {
            UpdateIdleSpeedByMovement();
            UpdateFeetAnimationByMovement();
        }

        private void StartIdleAnimation()
        {
            if (_bodyTransform == null)
                return;

            StopIdleAnimation();

            m_IdleTween = _bodyTransform
                .DOScale(new Vector3(_idleMoveX, _idleMoveY, 1f), _idleDuration)
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
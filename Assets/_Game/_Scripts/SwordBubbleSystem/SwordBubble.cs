using System;
using DG.Tweening;
using LoopGames;
using UnityEngine;

namespace _Game._Scripts.SwordBubbleSystem
{
    public class SwordBubble : MonoBehaviour, IPoolable
    {
        [Header("Sword Visual")]
        [SerializeField] private Transform _swordTransform;
        [SerializeField] private float _rotationSpeed = 180f;

        [Header("Pickup Animation")]
        [SerializeField] private Collider2D _triggerCollider;
        [SerializeField] private Transform _scaleTarget;

        [Tooltip("Default outward push distance when pickup starts.")]
        [SerializeField] private float _defaultPushDistance = 0.35f;

        [Tooltip("Default duration for outward push.")]
        [SerializeField] private float _defaultPushDuration = 0.15f;

        [Tooltip("Default small pause after push.")]
        [SerializeField] private float _defaultHoldDuration = 0.06f;

        [Tooltip("Default duration to move to center.")]
        [SerializeField] private float _defaultPullDuration = 0.28f;

        [Tooltip("Scale factor at the end of the pull (shrinks while approaching).")]
        [SerializeField] private float _defaultEndScaleMultiplier = 0.15f;

        [Tooltip("If true, disables collider immediately when pickup starts.")]
        [SerializeField] private bool _disableColliderOnPickup = true;

        [Tooltip("Movement easing (0..1). If empty, linear.")]
        [SerializeField] private AnimationCurve _moveEase;

        [Tooltip("Scale easing (0..1). If empty, linear.")]
        [SerializeField] private AnimationCurve _scaleEase;

        private Sequence m_PickupSequence;
        private Vector3 m_InitialScale;

        private void Awake()
        {
            m_InitialScale = GetScaleTarget().localScale;
        }

        private void OnDisable()
        {
            KillPickupSequence();
        }

        private void Update()
        {
            RotateSword();
        }

        private void RotateSword()
        {
            if (_swordTransform == null)
                return;

            _swordTransform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Plays a pickup sequence:
        /// 1) Pushes outward along radial direction from targetCenter,
        /// 2) Optional hold,
        /// 3) Pulls into targetCenter while shrinking.
        /// Calls onCompleted after the sequence finishes.
        /// 
        /// Each bubble runs its own animation, so multiple bubbles can be collected simultaneously.
        /// </summary>
        public void PlayPickupToCenter(
            Transform targetCenter,
            Action onCompleted = null,
            float? pushDistance = null,
            float? pushDuration = null,
            float? holdDuration = null,
            float? pullDuration = null,
            float? endScaleMultiplier = null)
        {
            if (targetCenter == null)
                return;

            if (_disableColliderOnPickup)
                SetColliderEnabled(false);

            KillPickupSequence();

            float finalPushDistance = pushDistance ?? _defaultPushDistance;
            float finalPushDuration = pushDuration ?? _defaultPushDuration;
            float finalHoldDuration = holdDuration ?? _defaultHoldDuration;
            float finalPullDuration = pullDuration ?? _defaultPullDuration;
            float finalEndScaleMultiplier = endScaleMultiplier ?? _defaultEndScaleMultiplier;

            Transform scaleTarget = GetScaleTarget();

            // Compute initial outward push using the target's current position at the moment of pickup.
            Vector3 startPos = transform.position;
            Vector3 centerPosAtStart = targetCenter.position;

            Vector3 dir = (startPos - centerPosAtStart);
            if (dir.sqrMagnitude < 0.0001f)
                dir = Vector3.right;
            dir.Normalize();

            Vector3 pushedPos = startPos + dir * finalPushDistance;

            Vector3 startScale = scaleTarget.localScale;
            Vector3 endScale = m_InitialScale * Mathf.Max(0.001f, finalEndScaleMultiplier);

            Vector3 pullStartPos = Vector3.zero;
            Vector3 pullStartScale = Vector3.zero;

            m_PickupSequence = DOTween.Sequence();

            // 1) Push outward
            if (finalPushDuration > 0f)
            {
                Tween pushTween = transform.DOMove(pushedPos, finalPushDuration);
                ApplyEase(pushTween, _moveEase, Ease.OutQuad);
                m_PickupSequence.Append(pushTween);
            }
            else
            {
                transform.position = pushedPos;
            }

            // 2) Hold
            if (finalHoldDuration > 0f)
                m_PickupSequence.AppendInterval(finalHoldDuration);

            // Capture pull start values exactly when pull begins.
            m_PickupSequence.AppendCallback(() =>
            {
                pullStartPos = transform.position;
                pullStartScale = scaleTarget.localScale;
            });

            // 3) Pull to center while shrinking, following the target transform until arrival.
            if (finalPullDuration > 0f)
            {
                Tween pullMoveTween = DOTween.To(
                    () => 0f,
                    t =>
                    {
                        // t is eased by DOTween; we still re-read targetCenter.position every update to follow.
                        Vector3 currentTarget = targetCenter != null ? targetCenter.position : pullStartPos;
                        transform.position = Vector3.LerpUnclamped(pullStartPos, currentTarget, t);
                    },
                    1f,
                    finalPullDuration);
                ApplyEase(pullMoveTween, _moveEase, Ease.InQuad);

                Tween pullScaleTween = DOTween.To(
                    () => 0f,
                    t =>
                    {
                        scaleTarget.localScale = Vector3.LerpUnclamped(pullStartScale, endScale, t);
                    },
                    1f,
                    finalPullDuration);
                ApplyEase(pullScaleTween, _scaleEase, Ease.InQuad);

                m_PickupSequence.Append(pullMoveTween);
                m_PickupSequence.Join(pullScaleTween);
            }
            else
            {
                transform.position = targetCenter.position;
                scaleTarget.localScale = endScale;
            }

            m_PickupSequence.OnComplete(() =>
            {
                // Ensure final snap to the latest target position.
                if (targetCenter != null)
                    transform.position = targetCenter.position;

                scaleTarget.localScale = endScale;
                m_PickupSequence = null;
                onCompleted?.Invoke();
            });
        }

        public void SetColliderEnabled(bool isEnabled)
        {
            var col = GetTriggerCollider();
            if (col != null)
                col.enabled = isEnabled;
        }

        public void ResetPickupVisuals()
        {
            KillPickupSequence();
            GetScaleTarget().localScale = m_InitialScale;
            SetColliderEnabled(true);
        }

        private void KillPickupSequence()
        {
            if (m_PickupSequence == null)
                return;

            m_PickupSequence.Kill();
            m_PickupSequence = null;
        }

        private static void ApplyEase(Tween tween, AnimationCurve curve, Ease fallback)
        {
            if (tween == null)
                return;

            if (curve != null && curve.length > 0)
                tween.SetEase(curve);
            else
                tween.SetEase(fallback);
        }

        private Collider2D GetTriggerCollider()
        {
            if (_triggerCollider != null)
                return _triggerCollider;

            _triggerCollider = GetComponent<Collider2D>();
            return _triggerCollider;
        }

        private Transform GetScaleTarget()
        {
            if (_scaleTarget != null)
                return _scaleTarget;

            // Default: scale the whole bubble if not specified.
            _scaleTarget = transform;
            return _scaleTarget;
        }

        public void OnSpawnedFromPool()
        {
            ResetPickupVisuals();
        }

        public void OnDespawnedToPool()
        {
            ResetPickupVisuals();
        }
    }
}
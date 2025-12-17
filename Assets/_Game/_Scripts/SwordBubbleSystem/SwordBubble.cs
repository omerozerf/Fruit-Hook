using System;
using _Game._Scripts.ObjectPoolSystem;
using DG.Tweening;
using UnityEngine;

namespace _Game._Scripts.SwordBubbleSystem
{
    public class SwordBubble : MonoBehaviour, IPoolable
    {
        [Header("Sword Visual")] [SerializeField]
        private Transform _swordTransform;

        [SerializeField] private float _rotationSpeed = 180f;

        [Header("Pickup Animation")] [SerializeField]
        private Collider2D _triggerCollider;

        [SerializeField] private Transform _scaleTarget;
        [SerializeField] private float _defaultPushDistance = 0.35f;
        [SerializeField] private float _defaultPushDuration = 0.15f;
        [SerializeField] private float _defaultHoldDuration = 0.06f;
        [SerializeField] private float _defaultPullDuration = 0.28f;
        [SerializeField] private float _defaultEndScaleMultiplier = 0.15f;
        [SerializeField] private bool _disableColliderOnPickup = true;
        [SerializeField] private AnimationCurve _moveEase;
        [SerializeField] private AnimationCurve _scaleEase;
        private Vector3 m_InitialScale;

        private Sequence m_PickupSequence;


        private void Awake()
        {
            m_InitialScale = GetScaleTarget().localScale;
        }

        private void Update()
        {
            RotateSword();
        }

        private void OnDisable()
        {
            KillPickupSequence();
        }

        public void OnSpawnedFromPool()
        {
            ResetPickupVisuals();
        }

        public void OnDespawnedToPool()
        {
            ResetPickupVisuals();
        }

        private void RotateSword()
        {
            if (_swordTransform == null)
                return;

            _swordTransform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);
        }

        /// <summary>
        ///     Plays a pickup sequence:
        ///     1) Pushes outward along radial direction from targetCenter,
        ///     2) Optional hold,
        ///     3) Pulls into targetCenter while shrinking.
        ///     Calls onCompleted after the sequence finishes.
        ///     Each bubble runs its own animation, so multiple bubbles can be collected simultaneously.
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

            var finalPushDistance = pushDistance ?? _defaultPushDistance;
            var finalPushDuration = pushDuration ?? _defaultPushDuration;
            var finalHoldDuration = holdDuration ?? _defaultHoldDuration;
            var finalPullDuration = pullDuration ?? _defaultPullDuration;
            var finalEndScaleMultiplier = endScaleMultiplier ?? _defaultEndScaleMultiplier;

            var scaleTarget = GetScaleTarget();

            var startPos = transform.position;
            var centerPosAtStart = targetCenter.position;

            var dir = startPos - centerPosAtStart;
            if (dir.sqrMagnitude < 0.0001f)
                dir = Vector3.right;
            dir.Normalize();

            var pushedPos = startPos + dir * finalPushDistance;

            var startScale = scaleTarget.localScale;
            var endScale = m_InitialScale * Mathf.Max(0.001f, finalEndScaleMultiplier);

            var pullStartPos = Vector3.zero;
            var pullStartScale = Vector3.zero;

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
                        var currentTarget = targetCenter ? targetCenter.position : pullStartPos;
                        transform.position = Vector3.LerpUnclamped(pullStartPos, currentTarget, t);
                    },
                    1f,
                    finalPullDuration);
                ApplyEase(pullMoveTween, _moveEase, Ease.InQuad);

                Tween pullScaleTween = DOTween.To(
                    () => 0f,
                    t => { scaleTarget.localScale = Vector3.LerpUnclamped(pullStartScale, endScale, t); },
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
                if (targetCenter)
                    transform.position = targetCenter.position;

                scaleTarget.localScale = endScale;
                m_PickupSequence = null;
                onCompleted?.Invoke();
            });
        }

        public void SetColliderEnabled(bool isEnabled)
        {
            var col = GetTriggerCollider();
            if (col)
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

            if (curve is { length: > 0 })
                tween.SetEase(curve);
            else
                tween.SetEase(fallback);
        }

        private Collider2D GetTriggerCollider()
        {
            if (_triggerCollider)
                return _triggerCollider;

            _triggerCollider = GetComponent<Collider2D>();
            return _triggerCollider;
        }

        private Transform GetScaleTarget()
        {
            if (_scaleTarget)
                return _scaleTarget;

            _scaleTarget = transform;
            return _scaleTarget;
        }
    }
}
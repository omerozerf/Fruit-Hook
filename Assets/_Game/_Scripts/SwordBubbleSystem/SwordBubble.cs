using System;
using _Game._Scripts.ObjectPoolSystem;
using _Game._Scripts.ScriptableObjects;
using DG.Tweening;
using UnityEngine;

namespace _Game._Scripts.SwordBubbleSystem
{
    public class SwordBubble : MonoBehaviour, IPoolable
    {
        [Header("Settings")] [SerializeField] private SwordBubbleSettingsSO _settings;

        [Header("Sword Visual")] [SerializeField]
        private Transform _swordTransform;

        [Header("Pickup Animation")] [SerializeField]
        private Collider2D _triggerCollider;

        [SerializeField] private Transform _scaleTarget;

        private Vector3 m_InitialScale;
        private Sequence m_PickupSequence;


        private void Awake()
        {
            if (!_settings)
            {
                Debug.LogError($"{nameof(SwordBubble)} on '{name}' has no {nameof(SwordBubbleSettingsSO)} assigned.");
                enabled = false;
                return;
            }

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


        public void PlayPickupToCenter(Transform targetCenter, Action onCompleted = null)
        {
            if (!targetCenter)
                return;

            if (_settings.DisableColliderOnPickup)
                SetColliderEnabled(false);

            KillPickupSequence();
            var finalPushDistance = _settings.DefaultPushDistance;
            var finalPushDuration = _settings.DefaultPushDuration;
            var finalHoldDuration = _settings.DefaultHoldDuration;
            var finalPullDuration = _settings.DefaultPullDuration;
            var finalEndScaleMultiplier = _settings.DefaultEndScaleMultiplier;

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
                ApplyEase(pushTween, _settings.MoveEase, Ease.OutQuad);
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
                ApplyEase(pullMoveTween, _settings.MoveEase, Ease.InQuad);

                Tween pullScaleTween = DOTween.To(
                    () => 0f,
                    t => { scaleTarget.localScale = Vector3.LerpUnclamped(pullStartScale, endScale, t); },
                    1f,
                    finalPullDuration);
                ApplyEase(pullScaleTween, _settings.ScaleEase, Ease.InQuad);

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


        private void RotateSword()
        {
            if (!_swordTransform)
                return;

            _swordTransform.Rotate(Vector3.forward, _settings.RotationSpeed * Time.deltaTime);
        }

        private void SetColliderEnabled(bool isEnabled)
        {
            var col = GetTriggerCollider();
            if (col)
                col.enabled = isEnabled;
        }

        private void ResetPickupVisuals()
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

        private void ApplyEase(Tween tween, AnimationCurve curve, Ease fallback)
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
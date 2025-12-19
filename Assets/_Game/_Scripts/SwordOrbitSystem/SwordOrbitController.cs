using System;
using System.Collections.Generic;
using _Game._Scripts.ObjectPoolSystem;
using _Game._Scripts.ScriptableObjects;
using _Game._Scripts.SwordOrbitSystem.Helpers;
using _Game._Scripts.SwordSystem;
using DG.Tweening;
using ScratchCardAsset;
using UnityEngine;

namespace _Game._Scripts.SwordOrbitSystem
{
    public sealed class SwordOrbitController : MonoBehaviour
    {
        [SerializeField] private bool _isPlayer;

        [Header("Settings")]
        [SerializeField] private SwordOrbitSettingsSO _settings;

        [Header("References")]
        [SerializeField] private ScratchCard _scratchCard;
        [SerializeField] private OrbitReferences _references = OrbitReferences.Default;

        private readonly SwordOrbitList m_OrbitList = new();
        private Camera m_Cam;
        private Tween m_CameraShakeTween;
        private SwordDespawnAnimator m_DespawnAnimator;
        private float m_RemoveTimer;
        private float m_SpawnTimer;
        private ObjectPool<Transform> m_SwordPool;

        [Header("Scratch Erase")]
        [SerializeField] private bool _enableSwordScratchErase = true;
        [SerializeField] private float _scratchBrushSize = 0.5f;
        [SerializeField, Range(0.01f, 1f)] private float _scratchPressure = 1f;

        private readonly List<Transform> m_ActiveSwords = new();
        private readonly Dictionary<int, Vector2> m_LastScratchPosBySwordId = new();

        
        private void Awake()
        {
            if (!_settings)
            {
                Debug.LogError($"{nameof(SwordOrbitController)} on '{name}' has no SwordOrbitSettings assigned.");
                enabled = false;
                return;
            }

            m_Cam = Camera.main;
            if (_references._swordPrefab)
                m_SwordPool = new ObjectPool<Transform>(
                    _references._swordPrefab,
                    transform,
                    Mathf.Max(0, _settings.PrewarmCount)
                );

            m_DespawnAnimator = new SwordDespawnAnimator(this, _settings.Despawn, OnSwordDespawnCompleted);
        }

        private void Start()
        {
            for (int i = 0; i < _settings.StartCount; i++)
            {
                SpawnSword();
            }
        }

        private void Update()
        {
            if (_settings.Test._enableTestSpawning)
                TickTestSpawning();

            if (_settings.Test._enableTestRemoval)
                TickTestRemoval();

            TickOrbit();
            TickRotation();
            TickSwordScratchErase();
        }
        

        private void PlayCameraShake()
        {
            if (!m_Cam) return;

            var camTransform = m_Cam.transform;

            m_CameraShakeTween?.Kill();

            m_CameraShakeTween = camTransform
                .DOShakePosition(
                    _settings.ShakeDuration,
                    new Vector3(_settings.ShakeStrength, _settings.ShakeStrength, 0f),
                    20
                )
                .SetUpdate(true);
        }

        private void OnSwordDespawnCompleted(Transform sword)
        {
            if (!sword)
                return;

            m_ActiveSwords.Remove(sword);
            m_LastScratchPosBySwordId.Remove(sword.GetInstanceID());

            if (m_SwordPool != null)
                m_SwordPool.Release(sword);
            else
                Destroy(sword.gameObject);
        }

        private void AddSwordToOrbit(Transform sword)
        {
            sword.GetComponent<Sword>().SetSwordOrbitController(this);
            m_OrbitList.Add(sword, _settings.Orbit._spawnGrowDuration);
            m_OrbitList.RecalculateTargetAngles();

            if (sword && !m_ActiveSwords.Contains(sword))
                m_ActiveSwords.Add(sword);
        }

        private void TickTestSpawning()
        {
            if (!_references._center || !_references._swordPrefab)
                return;

            m_SpawnTimer += Time.deltaTime;
            if (m_SpawnTimer < _settings.Test._spawnInterval)
                return;

            m_SpawnTimer = 0f;
            SpawnSword();
        }

        private void TickTestRemoval()
        {
            if (m_OrbitList.Count == 0)
                return;

            m_RemoveTimer += Time.deltaTime;
            if (m_RemoveTimer < _settings.Test._removeInterval)
                return;

            m_RemoveTimer = 0f;

            var lastSword = m_OrbitList.GetLastTransform();
            if (lastSword)
                RemoveSword(lastSword);
        }

        private void TickOrbit()
        {
            m_OrbitList.TickOrbit(
                Time.deltaTime,
                _settings.Orbit._radius,
                _settings.Orbit._smoothSpeed
            );
        }

        private void TickRotation()
        {
            if (Mathf.Abs(_settings.Orbit._rotationSpeed) < 0.0001f)
                return;

            transform.Rotate(0f, 0f, -_settings.Orbit._rotationSpeed * Time.deltaTime);
        }

        private void TickSwordScratchErase()
        {
            if (!_enableSwordScratchErase)
                return;

            if (!_scratchCard)
                return;

            if (!_scratchCard.Initialized)
                return;

            if (_scratchCard.ScratchData == null)
                return;

            // Prefer the camera that ScratchCard was initialized with (SpriteRendererData/MeshRendererData).
            var cam = _scratchCard.ScratchData.Camera ? _scratchCard.ScratchData.Camera : m_Cam;
            if (!cam)
                return;

            // Ensure brush size is what we expect (designer-tunable).
            if (Math.Abs(_scratchCard.BrushSize - _scratchBrushSize) > 0.0001f)
                _scratchCard.BrushSize = Mathf.Max(0.001f, _scratchBrushSize);

            ScratchWithActiveSwords();
        }

        private void ScratchWithActiveSwords()
        {
            // Iterate without allocations.
            for (int i = 0; i < m_ActiveSwords.Count; i++)
            {
                var sword = m_ActiveSwords[i];
                if (!sword)
                    continue;

                ScratchWithSwordTransform(sword);
            }
        }

        private void ScratchWithSwordTransform(Transform sword)
        {
            if (!sword)
                return;

            if (_scratchCard.ScratchData == null)
                return;

            var cam = _scratchCard.ScratchData.Camera ? _scratchCard.ScratchData.Camera : m_Cam;
            if (!cam)
                return;

            var surface = _scratchCard.SurfaceTransform;
            if (!surface)
                return;

            // Project sword position onto the scratch surface plane to avoid depth/parallax offsets.
            // For a SpriteRenderer surface, forward is typically the plane normal.
            var projectedWorld = ProjectPointToPlane(sword.position, surface.position, surface.forward);

            var screenPos3 = cam.WorldToScreenPoint(projectedWorld);
            if (screenPos3.z <= 0f)
                return;

            var screenPos = new Vector2(screenPos3.x, screenPos3.y);

            // Match the same pipeline as ScratchCardInput:
            // screen -> ScratchData.GetScratchPosition -> renderer scratch.
            var scratchPos = _scratchCard.ScratchData.GetScratchPosition(screenPos);

            var id = sword.GetInstanceID();

            if (m_LastScratchPosBySwordId.TryGetValue(id, out var lastScratchPos))
            {
                _scratchCard.ScratchLine(lastScratchPos, scratchPos, _scratchPressure, _scratchPressure);
                m_LastScratchPosBySwordId[id] = scratchPos;
            }
            else
            {
                _scratchCard.ScratchHole(scratchPos, _scratchPressure);
                m_LastScratchPosBySwordId.Add(id, scratchPos);
            }
        }

        private static Vector3 ProjectPointToPlane(Vector3 point, Vector3 planePoint, Vector3 planeNormal)
        {
            var n = planeNormal.sqrMagnitude > 0.000001f ? planeNormal.normalized : Vector3.forward;
            var dist = Vector3.Dot(point - planePoint, n);
            return point - dist * n;
        }
        
        public int GetSwordCount()
        {
            return m_OrbitList.Count;
        }
        
        public void SpawnSword()
        {
            if (!_references._swordPrefab)
                return;

            if (m_SwordPool == null)
                m_SwordPool = new ObjectPool<Transform>(
                    _references._swordPrefab,
                    transform
                );

            var swordInstance = m_SwordPool.Get(transform, false);
            swordInstance.localPosition = Vector3.zero;
            swordInstance.localRotation = Quaternion.identity;
            swordInstance.localScale = Vector3.zero;

            AddSwordToOrbit(swordInstance);
        }

        public void RemoveSword(Transform swordTransform)
        {
            if (!swordTransform)
                return;

            if (!m_OrbitList.TryRemove(swordTransform, out var _))
                return;

            m_OrbitList.RecalculateTargetAngles();

            m_ActiveSwords.Remove(swordTransform);
            m_LastScratchPosBySwordId.Remove(swordTransform.GetInstanceID());

            swordTransform.SetParent(null, true);
            var sword = swordTransform.GetComponent<Sword>();
            sword.SetSwordOrbitController(null);
            sword.SetColliderEnabled(false);
            m_DespawnAnimator.StartDespawn(sword, transform.position, _settings.Orbit._radius);

            if (_isPlayer) PlayCameraShake();
        }

        
        [Serializable]
        public struct OrbitSettings
        {
            public float _radius;
            public float _rotationSpeed;
            public float _smoothSpeed;
            public float _spawnGrowDuration;

            public static OrbitSettings Default => new()
            {
                _radius = 1.5f,
                _rotationSpeed = 90f,
                _smoothSpeed = 8f,
                _spawnGrowDuration = 0.25f
            };
        }

        [Serializable]
        public struct DespawnSettings
        {
            public float _duration;
            public float _spinSpeed;
            public float _offscreenMargin;

            public static DespawnSettings Default => new()
            {
                _duration = 0.35f,
                _spinSpeed = 1080f,
                _offscreenMargin = 1.5f
            };
        }

        [Serializable]
        private struct OrbitReferences
        {
            public Transform _center;
            public Transform _swordPrefab;

            public static OrbitReferences Default => new()
            {
                _center = null,
                _swordPrefab = null
            };
        }

        [Serializable]
        public struct TestSettings
        {
            public bool _enableTestSpawning;
            public float _spawnInterval;
            public bool _enableTestRemoval;
            public float _removeInterval;

            public static TestSettings Default => new()
            {
                _enableTestSpawning = false,
                _spawnInterval = 5f,
                _enableTestRemoval = false,
                _removeInterval = 5f
            };
        }
    }
}
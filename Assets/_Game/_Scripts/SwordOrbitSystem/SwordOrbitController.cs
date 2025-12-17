using System;
using _Game._Scripts.ObjectPoolSystem;
using _Game._Scripts.SwordOrbitSystem.Helpers;
using _Game._Scripts.SwordSystem;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game._Scripts.SwordOrbitSystem
{
    public sealed class SwordOrbitController : MonoBehaviour
    {
        [SerializeField] private bool _isPlayer;

        [Header("Settings")]
        [SerializeField] private SwordOrbitSettingsSO _settings;

        [Header("References")]
        [SerializeField] private OrbitReferences _references = OrbitReferences.Default;

        private Tween m_CameraShakeTween;

        private readonly SwordOrbitList m_OrbitList = new();
        private SwordDespawnAnimator m_DespawnAnimator;

        private ObjectPool<Transform> m_SwordPool;

        private float m_SpawnTimer;
        private float m_RemoveTimer;
        private Camera m_Cam;

        private void Awake()
        {
            if (!_settings)
            {
                Debug.LogError($"{nameof(SwordOrbitController)} on '{name}' has no SwordOrbitSettings assigned.");
                enabled = false;
                return;
            }

            m_Cam = Camera.main;
            if (_references._swordPrefab != null)
            {
                m_SwordPool = new ObjectPool<Transform>(
                    prefab: _references._swordPrefab,
                    parent: transform,
                    prewarmCount: Mathf.Max(0, _settings.PrewarmCount),
                    keepWorldPositionWhenParenting: false
                );
            }

            m_DespawnAnimator = new SwordDespawnAnimator(this, _settings.Despawn, OnSwordDespawnCompleted);
        }

        private void Update()
        {
            if (_settings.Test._enableTestSpawning)
                TickTestSpawning();

            if (_settings.Test._enableTestRemoval)
                TickTestRemoval();

            TickOrbit();
            TickRotation();
        }

        public void SpawnSword()
        {
            if (_references._swordPrefab == null)
                return;

            if (m_SwordPool == null)
            {
                m_SwordPool = new ObjectPool<Transform>(
                    prefab: _references._swordPrefab,
                    parent: transform,
                    prewarmCount: 0,
                    keepWorldPositionWhenParenting: false
                );
            }

            var swordInstance = m_SwordPool.Get(transform, worldPositionStays: false);
            swordInstance.localPosition = Vector3.zero;
            swordInstance.localRotation = Quaternion.identity;
            swordInstance.localScale = Vector3.zero;

            AddSwordToOrbit(swordInstance);
        }

        public void RemoveSword(Transform swordTransform)
        {
            if (!swordTransform)
                return;

            if (!m_OrbitList.TryRemove(swordTransform, out _))
                return;

            m_OrbitList.RecalculateTargetAngles();
            
            swordTransform.SetParent(null, true);
            var sword = swordTransform.GetComponent<Sword>();
            sword.SetSwordOrbitController(null);
            sword.SetColliderEnabled(false);
            m_DespawnAnimator.StartDespawn(swordTransform, transform.position, _settings.Orbit._radius);

            if (_isPlayer) PlayCameraShake();
        }
        
        private void PlayCameraShake()
        {
            if (!m_Cam) return;

            Transform camTransform = m_Cam.transform;

            m_CameraShakeTween?.Kill();

            m_CameraShakeTween = camTransform
                .DOShakePosition(
                    _settings.ShakeDuration,
                    new Vector3(_settings.ShakeStrength, _settings.ShakeStrength, 0f),
                    vibrato: 20,
                    randomness: 90f,
                    fadeOut: true
                )
                .SetUpdate(true);
        }

        private void OnSwordDespawnCompleted(Transform sword)
        {
            if (!sword)
                return;

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
                deltaTime: Time.deltaTime,
                radius: _settings.Orbit._radius,
                smoothSpeed: _settings.Orbit._smoothSpeed
            );
        }

        private void TickRotation()
        {
            if (Mathf.Abs(_settings.Orbit._rotationSpeed) < 0.0001f)
                return;

            transform.Rotate(0f, 0f, -_settings.Orbit._rotationSpeed * Time.deltaTime);
        }
        
        public int GetSwordCount()
        {
            return m_OrbitList.Count;
        }

        [Serializable]
        public struct OrbitSettings
        {
            public float _radius;
            public float _rotationSpeed;
            public float _smoothSpeed;
            public float _spawnGrowDuration;

            public static OrbitSettings Default => new OrbitSettings
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

            public static DespawnSettings Default => new DespawnSettings
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

            public static OrbitReferences Default => new OrbitReferences
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

            public static TestSettings Default => new TestSettings
            {
                _enableTestSpawning = false,
                _spawnInterval = 5f,
                _enableTestRemoval = false,
                _removeInterval = 5f
            };
        }
    }
}
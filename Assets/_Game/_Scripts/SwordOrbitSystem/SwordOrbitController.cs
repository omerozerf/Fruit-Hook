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
        [Header("Orbit Settings")]
        [SerializeField] private OrbitSettings _orbit = OrbitSettings.Default;

        [Header("Despawn Settings")]
        [SerializeField] private DespawnSettings _despawn = DespawnSettings.Default;

        [Header("References")]
        [SerializeField] private OrbitReferences _references = OrbitReferences.Default;

        [Header("Pooling")]
        [SerializeField] private int _prewarmCount = 8;

        [Header("Test Settings")]
        [SerializeField] private TestSettings _test = TestSettings.Default;
        
        [Header("Camera Shake")]
        [SerializeField] private float _shakeDuration = 0.3f;
        [SerializeField] private float _shakeStrength = 0.7f;

        private Tween m_CameraShakeTween;

        private readonly SwordOrbitList m_OrbitList = new();
        private SwordDespawnAnimator m_DespawnAnimator;

        private ObjectPool<Transform> m_SwordPool;

        private float m_SpawnTimer;
        private float m_RemoveTimer;
        private Camera m_Cam;

        private void Awake()
        {
            m_Cam = Camera.main;
            if (_references._swordPrefab != null)
            {
                m_SwordPool = new ObjectPool<Transform>(
                    prefab: _references._swordPrefab,
                    parent: transform,
                    prewarmCount: Mathf.Max(0, _prewarmCount),
                    keepWorldPositionWhenParenting: false
                );
            }

            m_DespawnAnimator = new SwordDespawnAnimator(this, _despawn, OnSwordDespawnCompleted);
        }

        private void Update()
        {
            if (_test._enableTestSpawning)
                TickTestSpawning();

            if (_test._enableTestRemoval)
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
            m_DespawnAnimator.StartDespawn(swordTransform, transform.position, _orbit._radius);

            if (_isPlayer) PlayCameraShake();
        }
        
        private void PlayCameraShake()
        {
            if (!m_Cam) return;

            Transform camTransform = m_Cam.transform;

            m_CameraShakeTween?.Kill();

            m_CameraShakeTween = camTransform
                .DOShakePosition(
                    _shakeDuration,
                    new Vector3(_shakeStrength, _shakeStrength, 0f),
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
            m_OrbitList.Add(sword, _orbit._spawnGrowDuration);
            m_OrbitList.RecalculateTargetAngles();
        }

        private void TickTestSpawning()
        {
            if (!_references._center || !_references._swordPrefab)
                return;

            m_SpawnTimer += Time.deltaTime;
            if (m_SpawnTimer < _test._spawnInterval)
                return;

            m_SpawnTimer = 0f;
            SpawnSword();
        }

        private void TickTestRemoval()
        {
            if (m_OrbitList.Count == 0)
                return;

            m_RemoveTimer += Time.deltaTime;
            if (m_RemoveTimer < _test._removeInterval)
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
                radius: _orbit._radius,
                smoothSpeed: _orbit._smoothSpeed
            );
        }

        private void TickRotation()
        {
            if (Mathf.Abs(_orbit._rotationSpeed) < 0.0001f)
                return;

            transform.Rotate(0f, 0f, -_orbit._rotationSpeed * Time.deltaTime);
        }

        [Serializable]
        private struct OrbitSettings
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
        private struct TestSettings
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
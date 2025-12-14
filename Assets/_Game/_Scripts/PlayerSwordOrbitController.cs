using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace LoopGames.Combat
{
    public sealed class PlayerSwordOrbitController : MonoBehaviour
    {
        [Header("Orbit Settings")]
        [SerializeField] private OrbitSettings _orbit = OrbitSettings.Default;

        [Header("Despawn Settings")]
        [SerializeField] private DespawnSettings _despawn = DespawnSettings.Default;

        [Header("References")]
        [SerializeField] private OrbitReferences _references = OrbitReferences.Default;

        [Header("Test Settings")]
        [SerializeField] private TestSettings _test = TestSettings.Default;

        private readonly SwordOrbitList m_OrbitList = new();
        private SwordDespawnAnimator m_DespawnAnimator;

        private float m_SpawnTimer;
        private float m_RemoveTimer;

        private void Awake()
        {
            m_DespawnAnimator = new SwordDespawnAnimator(this, _despawn);
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

            Transform swordInstance = Instantiate(_references._swordPrefab, transform);
            swordInstance.localPosition = Vector3.zero;
            swordInstance.localRotation = Quaternion.identity;
            swordInstance.localScale = Vector3.zero;

            AddSwordToOrbit(swordInstance);
        }

        public void RemoveSword(Transform sword)
        {
            if (sword == null)
                return;

            if (!m_OrbitList.TryRemove(sword, out _))
                return;

            // Spacing updates immediately.
            m_OrbitList.RecalculateTargetAngles();

            // Detach so orbit/rotation updates no longer affect despawn motion.
            sword.SetParent(null, true);

            m_DespawnAnimator.StartDespawn(sword, transform.position, _orbit._radius);
        }

        private void AddSwordToOrbit(Transform sword)
        {
            m_OrbitList.Add(sword, _orbit._spawnGrowDuration);
            m_OrbitList.RecalculateTargetAngles();
        }

        private void TickTestSpawning()
        {
            if (_references._center == null || _references._swordPrefab == null)
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

            Transform lastSword = m_OrbitList.GetLastTransform();
            if (lastSword != null)
                RemoveSword(lastSword);
        }

        private void TickOrbit()
        {
            m_OrbitList.TickOrbit(
                deltaTime: Time.deltaTime,
                radius: _orbit._radius,
                smoothTime: _orbit._smoothTime
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
            [FormerlySerializedAs("radius")] public float _radius;
            [FormerlySerializedAs("rotationSpeed")] public float _rotationSpeed;
            [FormerlySerializedAs("smoothTime")] public float _smoothTime;
            [FormerlySerializedAs("spawnGrowDuration")] public float _spawnGrowDuration;

            public static OrbitSettings Default => new OrbitSettings
            {
                _radius = 1.5f,
                _rotationSpeed = 90f,
                _smoothTime = 0.08f,
                _spawnGrowDuration = 0.25f
            };
        }

        [Serializable]
        public struct DespawnSettings
        {
            [FormerlySerializedAs("duration")] public float _duration;
            [FormerlySerializedAs("spinSpeed")] public float _spinSpeed;
            [FormerlySerializedAs("offscreenMargin")] public float _offscreenMargin;

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
            [FormerlySerializedAs("center")] public Transform _center;
            [FormerlySerializedAs("swordPrefab")] public Transform _swordPrefab;

            public static OrbitReferences Default => new OrbitReferences
            {
                _center = null,
                _swordPrefab = null
            };
        }

        [Serializable]
        private struct TestSettings
        {
            [FormerlySerializedAs("enableTestSpawning")] public bool _enableTestSpawning;
            [FormerlySerializedAs("spawnInterval")] public float _spawnInterval;

            [FormerlySerializedAs("enableTestRemoval")] public bool _enableTestRemoval;
            [FormerlySerializedAs("removeInterval")] public float _removeInterval;

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
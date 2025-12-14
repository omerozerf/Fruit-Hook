using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace LoopGames
{
    
    public sealed class SwordBubbleCreator : MonoBehaviour
    {
        [Header("Prefab & Pool")]
        [SerializeField] private Transform _swordBubblePrefab;
        [SerializeField] private Transform _poolParent;
        [SerializeField] private int _prewarmCount = 8;

        [Header("Spawn Timing (seconds)")]
        [SerializeField] private float _baseSpawnInterval = 3f;
        [SerializeField] private float _intervalVariation = 1.5f; // final = base + Random(-variation, +variation)
        [SerializeField] private bool _spawnOnStart = true;

        [Header("Spawn Area")]
        [SerializeField] private Collider2D _spawnArea2D;

        [SerializeField] private Vector2 _fallbackMin = new Vector2(-8f, -4f);
        [SerializeField] private Vector2 _fallbackMax = new Vector2(8f, 4f);

        [Header("Spawn Rules")]
        [SerializeField] private Transform _player;
        [SerializeField] private float _minDistanceFromPlayer = 1.5f;
        [SerializeField] private int _positionTryCount = 12;

        private ObjectPool<Transform> m_Pool;
        private Coroutine m_SpawnRoutine;
        private bool m_IsRunning;

        
        private void Awake()
        {
            if (_swordBubblePrefab == null)
            {
                Debug.LogError($"{nameof(SwordBubbleCreator)}: swordBubblePrefab is null.");
                enabled = false;
                return;
            }

            m_Pool = new ObjectPool<Transform>(
                prefab: _swordBubblePrefab,
                parent: _poolParent,
                prewarmCount: Mathf.Max(0, _prewarmCount),
                keepWorldPositionWhenParenting: false
            );
        }

        private void Start()
        {
            if (_spawnOnStart)
                StartSpawning();
        }

        private void OnDisable()
        {
            StopSpawning();
        }

        // ---- Public API ----

        public void StartSpawning()
        {
            if (m_IsRunning) return;

            m_IsRunning = true;
            m_SpawnRoutine = StartCoroutine(SpawnLoop());
        }

        public void StopSpawning()
        {
            if (!m_IsRunning) return;

            m_IsRunning = false;

            if (m_SpawnRoutine != null)
            {
                StopCoroutine(m_SpawnRoutine);
                m_SpawnRoutine = null;
            }
        }

        public Transform SpawnOne()
        {
            Vector3 pos = GetRandomSpawnPosition();
            return m_Pool.Get(pos, Quaternion.identity);
        }

        public void Release(Transform instance)
        {
            if (instance == null) return;
            m_Pool.Release(instance);
        }

        // ---- Internals ----

        private IEnumerator SpawnLoop()
        {
            while (m_IsRunning)
            {
                float wait = GetNextSpawnDelay();
                yield return new WaitForSeconds(wait);

                SpawnOne();
            }
        }

        private float GetNextSpawnDelay()
        {
            float v = _intervalVariation;
            float delay = _baseSpawnInterval + Random.Range(-v, v);
            return Mathf.Max(0.05f, delay);
        }

        private Vector3 GetRandomSpawnPosition()
        {
            // Try multiple times to satisfy min distance rule.
            for (int i = 0; i < Mathf.Max(1, _positionTryCount); i++)
            {
                Vector3 candidate = GetRandomPointInsideArea();

                if (_player == null || _minDistanceFromPlayer <= 0f)
                    return candidate;

                float dist = Vector2.Distance(candidate, _player.position);
                if (dist >= _minDistanceFromPlayer)
                    return candidate;
            }

            // Fallback: return whatever we get last (don’t hard fail).
            return GetRandomPointInsideArea();
        }

        private Vector3 GetRandomPointInsideArea()
        {
            if (_spawnArea2D != null)
            {
                Bounds b = _spawnArea2D.bounds;
                float x = Random.Range(b.min.x, b.max.x);
                float y = Random.Range(b.min.y, b.max.y);
                return new Vector3(x, y, 0f);
            }

            float rx = Random.Range(_fallbackMin.x, _fallbackMax.x);
            float ry = Random.Range(_fallbackMin.y, _fallbackMax.y);
            return new Vector3(rx, ry, 0f);
        }
    }
}
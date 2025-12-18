using System.Collections;
using _Game._Scripts.GameEvents;
using _Game._Scripts.MapSystem;
using _Game._Scripts.ObjectPoolSystem;
using _Game._Scripts.Patterns.EventBusPattern;
using _Game._Scripts.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game._Scripts.SwordBubbleSystem
{
    public sealed class SwordBubbleCreator : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private SwordBubbleCreatorSettingsSO _settings;

        [Header("Prefab & Pool")]
        [SerializeField] private Transform _swordBubblePrefab;
        [SerializeField] private Transform _poolParent;

        [Header("References")]
        [SerializeField] private Transform _player;
        
        private bool m_IsRunning;
        private EventBinding<PlayerDiedEvent> m_PlayerDiedEventBinding;
        private ObjectPool<Transform> m_Pool;
        private Coroutine m_SpawnRoutine;
        private EventBinding<SwordBubbleTaken> m_SwordBubbleTakenEventBinding;


        private void Awake()
        {
            if (!_settings)
            {
                Debug.LogError(
                    $"{nameof(SwordBubbleCreator)} on '{name}' has no {nameof(SwordBubbleCreatorSettingsSO)} assigned.");
                enabled = false;
                return;
            }

            if (!_swordBubblePrefab)
            {
                Debug.LogError($"{nameof(SwordBubbleCreator)}: swordBubblePrefab is null.");
                enabled = false;
                return;
            }

            m_Pool = new ObjectPool<Transform>(
                _swordBubblePrefab,
                _poolParent,
                Mathf.Max(0, _settings.PrewarmCount)
            );
        }

        private void Start()
        {
            if (_settings.SpawnOnStart)
                StartSpawning();
        }

        private void OnEnable()
        {
            m_PlayerDiedEventBinding = new EventBinding<PlayerDiedEvent>(HandlePlayerDied);
            EventBus<PlayerDiedEvent>.Subscribe(m_PlayerDiedEventBinding);
            
            m_SwordBubbleTakenEventBinding = new EventBinding<SwordBubbleTaken>(HandleSwordBubbleTaken);
            EventBus<SwordBubbleTaken>.Subscribe(m_SwordBubbleTakenEventBinding);
        }

        private void OnDisable()
        {
            StopSpawning();

            EventBus<PlayerDiedEvent>.Unsubscribe(m_PlayerDiedEventBinding);
            EventBus<SwordBubbleTaken>.Unsubscribe(m_SwordBubbleTakenEventBinding);
        }


        private void HandlePlayerDied(PlayerDiedEvent obj)
        {
            if (obj.isPlayer) return;

            SpawnCluster(obj.transform.position, _settings.DropCount, _settings.DropRadius);
        }

        private void HandleSwordBubbleTaken(SwordBubbleTaken obj)
        {
            Release(obj.transform);
        }

        
        private IEnumerator SpawnLoop()
        {
            while (m_IsRunning)
            {
                var wait = GetNextSpawnDelay();
                yield return new WaitForSeconds(wait);

                SpawnOne();
            }
        }

        private float GetNextSpawnDelay()
        {
            var v = _settings.IntervalVariation;
            var delay = _settings.BaseSpawnInterval + Random.Range(-v, v);
            const float minSpawnDelay = 0.05f;
            return Mathf.Max(minSpawnDelay, delay);
        }

        private Vector3 GetRandomSpawnPosition()
        {
            for (var i = 0; i < Mathf.Max(1, _settings.PositionTryCount); i++)
            {
                var candidate = GetRandomPointInsideArea();

                if (!_player || _settings.MinDistanceFromPlayer <= 0f)
                    return candidate;

                var dist = Vector2.Distance(candidate, _player.position);
                if (dist >= _settings.MinDistanceFromPlayer)
                    return candidate;
            }

            // Fallback: return whatever we get last (don’t hard fail).
            return GetRandomPointInsideArea();
        }

        private Vector3 GetRandomPointInsideArea()
        {
            var rx = Random.Range(1 , GridMapManager.Instance.GetWidth() - 1);
            var ry = Random.Range(1, GridMapManager.Instance.GetHeight() - 1);
            return new Vector3(rx, ry, 0f);
        }
        
        private void StartSpawning()
        {
            if (m_IsRunning) return;

            m_IsRunning = true;
            m_SpawnRoutine = StartCoroutine(SpawnLoop());
        }

        private void StopSpawning()
        {
            if (!m_IsRunning) return;

            m_IsRunning = false;

            if (m_SpawnRoutine != null)
            {
                StopCoroutine(m_SpawnRoutine);
                m_SpawnRoutine = null;
            }
        }

        private Transform SpawnOne()
        {
            var pos = GetRandomSpawnPosition();
            return m_Pool.Get(pos, Quaternion.identity);
        }

        private Transform SpawnAt(Vector3 position)
        {
            return m_Pool.Get(position, Quaternion.identity);
        }

        private void SpawnCluster(Vector3 centerPosition, int count, float radius)
        {
            if (count <= 0)
                return;

            radius = Mathf.Max(0f, radius);

            for (var i = 0; i < count; i++)
            {
                var offset = Random.insideUnitCircle * radius;
                var pos = centerPosition + new Vector3(offset.x, offset.y, 0f);
                SpawnAt(pos);
            }
        }

        private void Release(Transform instance)
        {
            if (!instance) return;
            m_Pool.Release(instance);
        }
    }
}
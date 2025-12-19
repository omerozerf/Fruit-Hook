using System.Collections.Generic;
using _Game._Scripts.MapSystem;
using _Game._Scripts.ScriptableObjects;
using _Game._Scripts.SwordOrbitSystem;
using ScratchCardAsset;
using UnityEngine;

namespace _Game._Scripts.EnemyMoveSystem
{
    public class EnemyCreateController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Transform[] _enemyPrefabArray;
        [SerializeField] private ScratchCard _scratchCard;

        [Header("Settings")]
        [SerializeField] private EnemyCreateSettingsSO _settings;

        private readonly List<Vector3> m_SpawnedEnemyPositions = new();
        private readonly List<ScoredPoint> m_TopCandidates = new();

        private int m_MapHeight;
        private int m_MapWidth;

        private void Awake()
        {
            if (!_settings)
            {
                Debug.LogError(
                    $"{nameof(EnemyCreateController)} on '{name}' has no {nameof(EnemyCreateSettingsSO)} assigned.");
                enabled = false;
                return;
            }

            CacheMapSize();

            if (_settings.UseFixedSeed)
                Random.InitState(_settings.FixedSeed);

            SpawnEnemies();
        }

        private void CacheMapSize()
        {
            m_MapWidth = GridMapManager.Instance.GetWidth();
            m_MapHeight = GridMapManager.Instance.GetHeight();
        }

        private void SpawnEnemies()
        {
            m_SpawnedEnemyPositions.Clear();

            foreach (var enemyPrefab in _enemyPrefabArray)
            {
                var spawnPosition = FindRandomBestSpawnPoint();
                m_SpawnedEnemyPositions.Add(spawnPosition);
                var transformEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                transformEnemy.GetComponentInChildren<SwordOrbitController>().SetScratchCard(_scratchCard);
            }
        }

        private Vector3 FindRandomBestSpawnPoint()
        {
            m_TopCandidates.Clear();

            var maxX = Mathf.Max(1, m_MapWidth) - 1;
            var maxY = Mathf.Max(1, m_MapHeight) - 1;

            for (var i = 0; i < _settings.CandidateSampleCount; i++)
            {
                var x = Random.Range(0, maxX + 1);
                var y = Random.Range(0, maxY + 1);
                var candidate = new Vector3(x, y, 0f);

                var score = CalculateSpawnScore(candidate);
                if (Mathf.Approximately(score, float.MinValue))
                    continue;

                TryAddTopCandidate(candidate, score);
            }

            if (m_TopCandidates.Count == 0)
                return FindBestSpawnPointDeterministic();

            var pickIndex = Random.Range(0, m_TopCandidates.Count);
            return m_TopCandidates[pickIndex].point;
        }

        private void TryAddTopCandidate(Vector3 point, float score)
        {
            var sp = new ScoredPoint(point, score);

            var insertIndex = m_TopCandidates.Count;
            for (var i = 0; i < m_TopCandidates.Count; i++)
                if (score > m_TopCandidates[i].score)
                {
                    insertIndex = i;
                    break;
                }

            m_TopCandidates.Insert(insertIndex, sp);

            var limit = Mathf.Max(1, _settings.TopCandidatesToChooseFrom);
            if (m_TopCandidates.Count > limit)
                m_TopCandidates.RemoveAt(m_TopCandidates.Count - 1);
        }

        private float CalculateSpawnScore(Vector3 candidate)
        {
            var score = Vector3.Distance(candidate, _playerTransform.position);

            // Diğer enemy’lerden yeterince uzak değilse eliyoruz
            foreach (var enemyPos in m_SpawnedEnemyPositions)
            {
                var distance = Vector3.Distance(candidate, enemyPos);

                if (distance < _settings.MinEnemyDistance)
                    return float.MinValue;

                // Enemy’lerden de uzak olmayı ödüllendir
                score += distance;
            }

            return score;
        }

        private Vector3 FindBestSpawnPointDeterministic()
        {
            var bestPoint = Vector3.zero;
            var bestScore = float.MinValue;

            var maxX = Mathf.Max(1, m_MapWidth) - 1;
            var maxY = Mathf.Max(1, m_MapHeight) - 1;

            for (var x = 0; x <= maxX; x++)
            for (var y = 0; y <= maxY; y++)
            {
                var candidate = new Vector3(x, y, 0f);
                var score = CalculateSpawnScore(candidate);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestPoint = candidate;
                }
            }

            return bestPoint;
        }

        private struct ScoredPoint
        {
            public readonly Vector3 point;
            public readonly float score;

            public ScoredPoint(Vector3 point, float score)
            {
                this.point = point;
                this.score = score;
            }
        }
    }
}
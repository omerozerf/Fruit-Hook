using System.Collections.Generic;
using _Game._Scripts.MapSystem;
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

        [Header("Spawn Randomness")]
        [SerializeField, Min(1)] private int _candidateSampleCount = 300;
        [SerializeField, Min(1)] private int _topCandidatesToChooseFrom = 10;

        [Header("Constraints")]
        [SerializeField, Min(0f)] private float _minEnemyDistance = 3f;

        [Header("Optional Determinism")]
        [SerializeField] private bool _useFixedSeed;
        [SerializeField] private int _fixedSeed = 12345;

        private readonly List<Vector3> m_SpawnedEnemyPositions = new();
        private readonly List<ScoredPoint> m_TopCandidates = new();

        private int m_MapHeight;
        private int m_MapWidth;

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

        private void Awake()
        {
            CacheMapSize();

            if (_useFixedSeed)
                Random.InitState(_fixedSeed);

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
                var transform = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                transform.GetComponentInChildren<SwordOrbitController>().SetScratchCard(_scratchCard);
            }
        }

        private Vector3 FindRandomBestSpawnPoint()
        {
            m_TopCandidates.Clear();

            // Map sınırları: 0..width-1, 0..height-1 (<= değil <)
            var maxX = Mathf.Max(1, m_MapWidth) - 1;
            var maxY = Mathf.Max(1, m_MapHeight) - 1;

            // Rastgele aday örnekle
            for (var i = 0; i < _candidateSampleCount; i++)
            {
                var x = Random.Range(0, maxX + 1);
                var y = Random.Range(0, maxY + 1);
                var candidate = new Vector3(x, y, 0f);

                var score = CalculateSpawnScore(candidate);
                if (score == float.MinValue)
                    continue;

                TryAddTopCandidate(candidate, score);
            }

            // Eğer hiç geçerli aday yoksa fallback: deterministik tarama
            if (m_TopCandidates.Count == 0)
                return FindBestSpawnPointDeterministic();

            // En iyi N adayın içinden rastgele seç
            var pickIndex = Random.Range(0, m_TopCandidates.Count);
            return m_TopCandidates[pickIndex].point;
        }

        private void TryAddTopCandidate(Vector3 point, float score)
        {
            // Listeyi (küçük N) score'a göre azalan tutuyoruz.
            // N küçük olduğu için basit insertion yeterli.
            var sp = new ScoredPoint(point, score);

            var insertIndex = m_TopCandidates.Count;
            for (var i = 0; i < m_TopCandidates.Count; i++)
            {
                if (score > m_TopCandidates[i].score)
                {
                    insertIndex = i;
                    break;
                }
            }

            m_TopCandidates.Insert(insertIndex, sp);

            var limit = Mathf.Max(1, _topCandidatesToChooseFrom);
            if (m_TopCandidates.Count > limit)
                m_TopCandidates.RemoveAt(m_TopCandidates.Count - 1);
        }

        private float CalculateSpawnScore(Vector3 candidate)
        {
            // Player'dan uzaklık temel skor
            var score = Vector3.Distance(candidate, _playerTransform.position);

            // Diğer enemy’lerden yeterince uzak değilse eliyoruz
            foreach (var enemyPos in m_SpawnedEnemyPositions)
            {
                var distance = Vector3.Distance(candidate, enemyPos);

                if (distance < _minEnemyDistance)
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
    }
}
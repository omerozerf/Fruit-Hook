using System.Collections.Generic;
using _Game._Scripts.MapSystem;
using UnityEngine;

namespace _Game._Scripts.EnemyMoveSystem
{
    public class EnemyCreateController : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Transform[] _enemyPrefabArray;

        private readonly List<Vector3> m_SpawnedEnemyPositions = new();
        private int m_MapHeight;

        private int m_MapWidth;


        private void Awake()
        {
            CacheMapSize();
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
                var spawnPosition = FindBestSpawnPoint();
                m_SpawnedEnemyPositions.Add(spawnPosition);
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            }
        }

        private Vector3 FindBestSpawnPoint()
        {
            var bestPoint = Vector3.zero;
            var bestScore = float.MinValue;

            for (var x = 0; x <= m_MapWidth; x++)
            for (var y = 0; y <= m_MapHeight; y++)
            {
                var candidate = new Vector3(x, y, 0);

                var score = CalculateSpawnScore(candidate);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestPoint = candidate;
                }
            }

            return bestPoint;
        }

        private float CalculateSpawnScore(Vector3 candidate)
        {
            const float minEnemyDistance = 3f;

            var score = Vector3.Distance(candidate, _playerTransform.position);

            foreach (var enemyPos in m_SpawnedEnemyPositions)
            {
                var distance = Vector3.Distance(candidate, enemyPos);

                if (distance < minEnemyDistance)
                    return float.MinValue;

                score += distance;
            }

            return score;
        }

        private Vector3 FindFarthestPointFromPlayer()
        {
            var playerPos = _playerTransform.position;

            Vector3[] candidatePoints =
            {
                new(0, 0, 0),
                new(m_MapWidth, 0, 0),
                new(0, m_MapHeight, 0),
                new(m_MapWidth, m_MapHeight, 0)
            };

            var farthestPoint = candidatePoints[0];
            var maxDistance = Vector3.Distance(playerPos, farthestPoint);

            for (var i = 1; i < candidatePoints.Length; i++)
            {
                var distance = Vector3.Distance(playerPos, candidatePoints[i]);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    farthestPoint = candidatePoints[i];
                }
            }

            return farthestPoint;
        }
    }
}
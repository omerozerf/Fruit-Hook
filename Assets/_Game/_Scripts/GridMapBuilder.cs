using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game._Scripts
{
    public sealed class GridMapBuilder : MonoBehaviour
    {
        [Serializable]
        private struct WeightedPrefab
        {
            [Min(0)] public int _weight;
            public Transform _prefab;
        }

        [Header("Grid")]
        [Min(1)] [SerializeField] private int _width = 10;
        [Min(1)] [SerializeField] private int _height = 10;
        [Min(0.01f)] [SerializeField] private float _cellSize = 1f;

        [Header("Ground Prefabs (Weighted)")]
        [SerializeField] private WeightedPrefab[] _groundVariants;

        [Header("Fence")]
        [SerializeField] private Transform _fenceVertical;
        [SerializeField] private Transform _fenceHorizontal;
        [SerializeField] private Transform _fenceCorner;

        [Header("Global Sorting")]
        [Tooltip("Assigned in decreasing order. Example: 0, -1, -2 ... (negative values are OK).")]
        [SerializeField] private int _startSortingOrder = 0;

        private readonly List<Transform> m_SpawnedRoots = new List<Transform>(512);

        
        private void Awake()
        {
            m_SpawnedRoots.Clear();

            BuildGround();
            BuildFence();

            ApplyGlobalSorting();
        }

        
        private void BuildGround()
        {
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    var prefab = GetRandomWeightedGround();
                    if (!prefab)
                        continue;

                    var instance = Instantiate(prefab, GridToWorld(x, y), Quaternion.identity, transform);
                    m_SpawnedRoots.Add(instance);
                }
            }
        }

        private void BuildFence()
        {
            if (!_fenceCorner || !_fenceHorizontal || !_fenceVertical)
            {
                Debug.LogError(
                    "Fence prefabs are not fully assigned (GridMapBuilder requires: _fenceCorner, _fenceHorizontal, _fenceVertical).",
                    this
                );
                return;
            }

            // Corners (4)
            var c0 = Instantiate(_fenceCorner, GridToWorld(-1, -1), Quaternion.identity, transform);
            m_SpawnedRoots.Add(c0);
            var c1 = Instantiate(_fenceCorner, GridToWorld(_width, -1), Quaternion.identity, transform);
            m_SpawnedRoots.Add(c1);
            var c2 = Instantiate(_fenceCorner, GridToWorld(-1, _height), Quaternion.identity, transform);
            m_SpawnedRoots.Add(c2);
            var c3 = Instantiate(_fenceCorner, GridToWorld(_width, _height), Quaternion.identity, transform);
            m_SpawnedRoots.Add(c3);

            // Bottom & Top edges (horizontal) - exclude corners
            for (var x = 0; x < _width; x++)
            {
                var h0 = Instantiate(_fenceHorizontal, GridToWorld(x, -1), Quaternion.identity, transform);
                m_SpawnedRoots.Add(h0);
                var h1 = Instantiate(_fenceHorizontal, GridToWorld(x, _height), Quaternion.identity, transform);
                m_SpawnedRoots.Add(h1);
            }

            // Left & Right edges (vertical) - exclude corners
            for (var y = 0; y < _height; y++)
            {
                var v0 = Instantiate(_fenceVertical, GridToWorld(-1, y), Quaternion.identity, transform);
                m_SpawnedRoots.Add(v0);
                var v1 = Instantiate(_fenceVertical, GridToWorld(_width, y), Quaternion.identity, transform);
                m_SpawnedRoots.Add(v1);
            }
        }

        private void ApplyGlobalSorting()
        {
            var order = _startSortingOrder;

            for (var i = 0; i < m_SpawnedRoots.Count; i++)
            {
                var root = m_SpawnedRoots[i];
                if (!root)
                    continue;

                ApplySortingForRoot(root, ref order);
            }
        }

        private void ApplySortingForRoot(Transform root, ref int globalOrder)
        {
            // Rule: for every prefab, skip its first child (index 0).
            // Then, for each remaining child (in hierarchy order), if it has a SpriteRenderer,
            // assign sortingOrder from a SINGLE global decreasing counter.
            var childCount = root.childCount;
            for (var i = 1; i < childCount; i++)
            {
                var child = root.GetChild(i);
                if (!child)
                    continue;

                if (!child.TryGetComponent<SpriteRenderer>(out var sr))
                    continue;

                sr.sortingOrder = globalOrder;
                globalOrder -= 1;
            }
        }

        private Vector3 GridToWorld(int x, int y)
        {
            return new Vector3(x * _cellSize, y * _cellSize, 0f);
        }

        private Transform GetRandomWeightedGround()
        {
            if (_groundVariants == null || _groundVariants.Length == 0)
            {
                Debug.LogError("No ground variants assigned in GridMapBuilder.", this);
                return null;
            }

            var totalWeight = 0;
            for (var i = 0; i < _groundVariants.Length; i++)
            {
                var w = _groundVariants[i]._weight;
                if (w > 0 && _groundVariants[i]._prefab != null)
                    totalWeight += w;
            }

            if (totalWeight <= 0)
            {
                Debug.LogError("All ground variant weights are 0 or prefabs are missing.", this);
                return null;
            }

            var roll = UnityEngine.Random.Range(0, totalWeight);
            for (var i = 0; i < _groundVariants.Length; i++)
            {
                var entry = _groundVariants[i];
                if (entry._weight <= 0 || !entry._prefab)
                    continue;

                roll -= entry._weight;
                if (roll < 0)
                    return entry._prefab;
            }

            // Fallback (should never happen)
            return _groundVariants[0]._prefab;
        }
    }
}
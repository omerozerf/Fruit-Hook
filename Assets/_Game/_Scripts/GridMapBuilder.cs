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

        [Header("Extra Padding")]
        [Tooltip("Number of extra tile rows/columns to generate outside the main grid on each side.")]
        [Min(0)] [SerializeField] private int _extraPaddingCells = 0;

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
            // 1) Main grid (your intended bounds)
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    SpawnGroundAt(x, y);
                }
            }

            // 2) Extra padding outside the bounds (to avoid camera empty space)
            if (_extraPaddingCells <= 0)
                return;

            var startX = -_extraPaddingCells;
            var endX = _width + _extraPaddingCells;
            var startY = -_extraPaddingCells;
            var endY = _height + _extraPaddingCells;

            for (var x = startX; x < endX; x++)
            {
                for (var y = startY; y < endY; y++)
                {
                    // Skip cells that are inside the main grid.
                    if (x >= 0 && x < _width && y >= 0 && y < _height)
                        continue;

                    SpawnGroundAt(x, y);
                }
            }
        }

        private void SpawnGroundAt(int x, int y)
        {
            var prefab = GetRandomWeightedGround();
            if (!prefab)
                return;

            var instance = Instantiate(prefab, GridToWorld(x, y), Quaternion.identity, transform);
            m_SpawnedRoots.Add(instance);
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

            var minX = 0;
            var maxX = _width - 1;
            var minY = 0;
            var maxY = _height - 1;

            // Corners
            m_SpawnedRoots.Add(Instantiate(_fenceCorner, GridToWorld(minX - 1, minY - 1), Quaternion.identity, transform));
            m_SpawnedRoots.Add(Instantiate(_fenceCorner, GridToWorld(maxX + 1, minY - 1), Quaternion.identity, transform));
            m_SpawnedRoots.Add(Instantiate(_fenceCorner, GridToWorld(minX - 1, maxY + 1), Quaternion.identity, transform));
            m_SpawnedRoots.Add(Instantiate(_fenceCorner, GridToWorld(maxX + 1, maxY + 1), Quaternion.identity, transform));

            // Horizontal edges
            for (var x = minX; x <= maxX; x++)
            {
                Instantiate(_fenceHorizontal, GridToWorld(x, minY - 1), Quaternion.identity, transform);
                Instantiate(_fenceHorizontal, GridToWorld(x, maxY + 1), Quaternion.identity, transform);
            }

            // Vertical edges
            for (var y = minY; y <= maxY; y++)
            {
                m_SpawnedRoots.Add(Instantiate(_fenceVertical, GridToWorld(minX - 1, y), Quaternion.identity, transform));
                m_SpawnedRoots.Add(Instantiate(_fenceVertical, GridToWorld(maxX + 1, y), Quaternion.identity, transform));
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
            // Then, for each remaining child (in hierarchy order), assign sortingOrder using a SINGLE global decreasing counter
            // to every SpriteRenderer found under that child (including grandchildren).
            var childCount = root.childCount;
            for (var i = 1; i < childCount; i++)
            {
                var child = root.GetChild(i);
                if (!child)
                    continue;

                var renderers = child.GetComponentsInChildren<SpriteRenderer>(true);
                for (var r = 0; r < renderers.Length; r++)
                {
                    var sr = renderers[r];
                    if (!sr)
                        continue;

                    sr.sortingOrder = globalOrder;
                    globalOrder -= 1;
                }
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
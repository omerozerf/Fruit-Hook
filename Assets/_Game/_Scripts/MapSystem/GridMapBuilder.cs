using System;
using System.Collections.Generic;
using _Game._Scripts.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game._Scripts.MapSystem
{
    public sealed class GridMapBuilder : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private GridMapBuilderSettingsSO _settings;

        private readonly List<Transform> m_SpawnedRoots = new(512);
        private readonly List<Transform> m_TopCornerRoots = new(4);


        private void Awake()
        {
            if (!_settings)
            {
                Debug.LogError(
                    $"{nameof(GridMapBuilder)} on '{name}' has no {nameof(GridMapBuilderSettingsSO)} assigned.", this);
                enabled = false;
                return;
            }

            m_SpawnedRoots.Clear();
            m_TopCornerRoots.Clear();

            BuildGround();
            BuildFence();

            ApplyGlobalSorting();
            DisableTopCornerLastRenderers();
        }


        private void BuildGround()
        {
            // 1) Main grid (your intended bounds)
            for (var x = 0; x < _settings.Width; x++)
            for (var y = 0; y < _settings.Height; y++)
                SpawnGroundAt(x, y);

            // 2) Extra padding outside the bounds (to avoid camera empty space)
            if (_settings.ExtraPaddingCells <= 0)
                return;

            var startX = -_settings.ExtraPaddingCells;
            var endX = _settings.Width + _settings.ExtraPaddingCells;
            var startY = -_settings.ExtraPaddingCells;
            var endY = _settings.Height + _settings.ExtraPaddingCells;

            for (var x = startX; x < endX; x++)
            for (var y = startY; y < endY; y++)
            {
                // Skip cells that are inside the main grid.
                if (x >= 0 && x < _settings.Width && y >= 0 && y < _settings.Height)
                    continue;

                SpawnGroundAt(x, y);
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
            if (!_settings.FenceCorner || !_settings.FenceHorizontal || !_settings.FenceVertical)
            {
                Debug.LogError(
                    "Fence prefabs are not fully assigned (GridMapBuilder requires: _fenceCorner, _fenceHorizontal, _fenceVertical).",
                    this
                );
                return;
            }

            var minX = 0;
            var maxX = _settings.Width - 1;
            var minY = 0;
            var maxY = _settings.Height - 1;

            // Corners
            var bottomLeft = Instantiate(_settings.FenceCorner, GridToWorld(minX - 1, minY - 1), Quaternion.identity,
                transform);
            m_SpawnedRoots.Add(bottomLeft);

            // Bottom-right corner should be vertical (no corner prefab)
            var bottomRight = Instantiate(_settings.FenceVertical, GridToWorld(maxX + 1, minY - 1), Quaternion.identity,
                transform);
            m_SpawnedRoots.Add(bottomRight);

            // Top-left corner should be vertical
            var topLeft = Instantiate(_settings.FenceVertical, GridToWorld(minX - 1, maxY + 1), Quaternion.identity,
                transform);
            m_SpawnedRoots.Add(topLeft);
            m_TopCornerRoots.Add(topLeft);

            // Top-right corner should be vertical
            var topRight = Instantiate(_settings.FenceVertical, GridToWorld(maxX + 1, maxY + 1), Quaternion.identity,
                transform);
            m_SpawnedRoots.Add(topRight);
            m_TopCornerRoots.Add(topRight);

            // Horizontal edges
            for (var x = minX; x <= maxX; x++)
            {
                Instantiate(_settings.FenceHorizontal, GridToWorld(x, minY - 1), Quaternion.identity, transform);
                Instantiate(_settings.FenceHorizontal, GridToWorld(x, maxY + 1), Quaternion.identity, transform);
            }

            // Vertical edges
            for (var y = minY; y <= maxY; y++)
            {
                m_SpawnedRoots.Add(Instantiate(_settings.FenceVertical, GridToWorld(minX - 1, y), Quaternion.identity,
                    transform));
                m_SpawnedRoots.Add(Instantiate(_settings.FenceVertical, GridToWorld(maxX + 1, y), Quaternion.identity,
                    transform));
            }
        }

        private void ApplyGlobalSorting()
        {
            var order = _settings.StartSortingOrder;

            // 1) Sort everything EXCEPT top corners first
            for (var i = 0; i < m_SpawnedRoots.Count; i++)
            {
                var root = m_SpawnedRoots[i];
                if (!root)
                    continue;

                if (m_TopCornerRoots.Contains(root))
                    continue;

                ApplySortingForRoot(root, ref order);
            }

            // 2) Sort top corners LAST, continuing from the remaining global order
            for (var i = 0; i < m_TopCornerRoots.Count; i++)
            {
                var root = m_TopCornerRoots[i];
                if (!root)
                    continue;

                ApplySortingForRoot(root, ref order);
            }
        }

        private void DisableTopCornerLastRenderers()
        {
            for (var i = 0; i < m_TopCornerRoots.Count; i++)
            {
                var root = m_TopCornerRoots[i];
                if (!root)
                    continue;

                DisableLastRendererInSortingTraversal(root);
            }
        }

        private void DisableLastRendererInSortingTraversal(Transform root)
        {
            // “Last” means: the last SpriteRenderer encountered by the SAME traversal order we use for sorting
            // (skip child(0), then walk children in order, then GetComponentsInChildren order).
            SpriteRenderer last = null;

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

                    last = sr;
                }
            }

            if (last) last.enabled = false;
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
            return new Vector3(x * _settings.CellSize, y * _settings.CellSize, 0f);
        }

        private Transform GetRandomWeightedGround()
        {
            if (_settings.GroundVariants == null || _settings.GroundVariants.Length == 0)
            {
                Debug.LogError("No ground variants assigned in GridMapBuilder.", this);
                return null;
            }

            var totalWeight = 0;
            for (var i = 0; i < _settings.GroundVariants.Length; i++)
            {
                var w = _settings.GroundVariants[i]._weight;
                if (w > 0 && _settings.GroundVariants[i]._prefab)
                    totalWeight += w;
            }

            if (totalWeight <= 0)
            {
                Debug.LogError("All ground variant weights are 0 or prefabs are missing.", this);
                return null;
            }

            var roll = Random.Range(0, totalWeight);
            for (var i = 0; i < _settings.GroundVariants.Length; i++)
            {
                var entry = _settings.GroundVariants[i];
                if (entry._weight <= 0 || !entry._prefab)
                    continue;

                roll -= entry._weight;
                if (roll < 0)
                    return entry._prefab;
            }

            // Fallback (should never happen)
            return _settings.GroundVariants[0]._prefab;
        }

        
        [Serializable]
        public struct WeightedPrefab
        {
            [Min(0)] public int _weight;
            public Transform _prefab;
        }
    }
}
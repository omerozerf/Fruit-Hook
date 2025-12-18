using System.Collections.Generic;
using _Game._Scripts.ScriptableObjects;
using UnityEngine;

namespace _Game._Scripts.MapSystem.Helpers
{
    internal sealed class ChunkRootProvider
    {
        private readonly Dictionary<Vector2Int, Transform> m_ChunkRoots = new();
        private readonly Transform m_Parent;
        private readonly GridMapBuilderSettingsSO m_Settings;

        
        public ChunkRootProvider(Transform parent, GridMapBuilderSettingsSO settings)
        {
            m_Parent = parent;
            m_Settings = settings;
            ChunkWorldSize = m_Settings.CellSize * Mathf.Max(1, m_Settings.ChunkSizeInCells);
        }

        public IReadOnlyDictionary<Vector2Int, Transform> ChunkRoots => m_ChunkRoots;
        public float ChunkWorldSize { get; }

        public Transform GetOrCreateChunkRoot(int gridX, int gridY)
        {
            var key = GridToChunkKey(gridX, gridY);
            if (m_ChunkRoots.TryGetValue(key, out var existing) && existing)
                return existing;

            var go = new GameObject($"chunk_{key.x}_{key.y}");
            go.transform.SetParent(m_Parent, false);
            var t = go.transform;
            m_ChunkRoots[key] = t;
            return t;
        }

        public Vector3 GridToWorld(int x, int y)
        {
            return new Vector3(x * m_Settings.CellSize, y * m_Settings.CellSize, 0f);
        }

        private Vector2Int GridToChunkKey(int gridX, int gridY)
        {
            var size = Mathf.Max(1, m_Settings.ChunkSizeInCells);
            var cx = Mathf.FloorToInt(gridX / (float)size);
            var cy = Mathf.FloorToInt(gridY / (float)size);
            return new Vector2Int(cx, cy);
        }
    }
}
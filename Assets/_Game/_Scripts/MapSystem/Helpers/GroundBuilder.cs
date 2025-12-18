using _Game._Scripts.ScriptableObjects;
using UnityEngine;

namespace _Game._Scripts.MapSystem.Helpers
{
    internal sealed class GroundBuilder
    {
        private readonly BuildContext m_Ctx;
        private readonly ChunkRootProvider m_Roots;
        private readonly WeightedGroundSelector m_Selector;
        private readonly GridMapBuilderSettingsSO m_Settings;

        public GroundBuilder(
            GridMapBuilderSettingsSO settings,
            ChunkRootProvider roots,
            WeightedGroundSelector selector,
            BuildContext ctx)
        {
            m_Settings = settings;
            m_Roots = roots;
            m_Selector = selector;
            m_Ctx = ctx;
        }

        public void Build()
        {
            BuildMainGrid();
            BuildPaddingIfAny();
        }

        private void BuildMainGrid()
        {
            for (var x = 0; x < m_Settings.Width; x++)
            for (var y = 0; y < m_Settings.Height; y++)
                SpawnGroundAt(x, y);
        }

        private void BuildPaddingIfAny()
        {
            if (m_Settings.ExtraPaddingCells <= 0)
                return;

            var startX = -m_Settings.ExtraPaddingCells;
            var endX = m_Settings.Width + m_Settings.ExtraPaddingCells;
            var startY = -m_Settings.ExtraPaddingCells;
            var endY = m_Settings.Height + m_Settings.ExtraPaddingCells;

            for (var x = startX; x < endX; x++)
            for (var y = startY; y < endY; y++)
            {
                if (IsInsideMainGrid(x, y))
                    continue;

                SpawnGroundAt(x, y);
            }
        }

        private bool IsInsideMainGrid(int x, int y)
        {
            return x >= 0 && x < m_Settings.Width && y >= 0 && y < m_Settings.Height;
        }

        private void SpawnGroundAt(int x, int y)
        {
            var prefab = m_Selector.Pick();
            if (!prefab)
                return;

            var parent = m_Roots.GetOrCreateChunkRoot(x, y);
            var instance = Object.Instantiate(prefab, m_Roots.GridToWorld(x, y), Quaternion.identity, parent);
            m_Ctx.spawnedRoots.Add(instance);
        }
    }
}
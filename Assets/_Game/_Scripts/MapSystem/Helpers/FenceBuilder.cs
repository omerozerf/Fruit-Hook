using _Game._Scripts.ScriptableObjects;
using UnityEngine;

namespace _Game._Scripts.MapSystem.Helpers
{
    internal sealed class FenceBuilder
    {
        private readonly BuildContext m_Ctx;
        private readonly ChunkRootProvider m_Roots;
        private readonly GridMapBuilderSettingsSO m_Settings;

        public FenceBuilder(GridMapBuilderSettingsSO settings, ChunkRootProvider roots, BuildContext ctx)
        {
            m_Settings = settings;
            m_Roots = roots;
            m_Ctx = ctx;
        }

        
        public void Build()
        {
            if (!AreFencePrefabsValid())
                return;

            var minX = 0;
            var maxX = m_Settings.Width - 1;
            var minY = 0;
            var maxY = m_Settings.Height - 1;

            SpawnCorners(minX, maxX, minY, maxY);
            SpawnHorizontalEdges(minX, maxX, minY, maxY);
            SpawnVerticalEdges(minX, maxX, minY, maxY);
        }

        private bool AreFencePrefabsValid()
        {
            if (m_Settings.FenceCorner && m_Settings.FenceHorizontal && m_Settings.FenceVertical)
                return true;

            Debug.LogError(
                "Fence prefabs are not fully assigned (GridMapBuilder requires: _fenceCorner, _fenceHorizontal, _fenceVertical)."
            );
            return false;
        }

        private void SpawnCorners(int minX, int maxX, int minY, int maxY)
        {
            // Bottom-left: corner
            var bottomLeft = Object.Instantiate(
                m_Settings.FenceCorner,
                m_Roots.GridToWorld(minX - 1, minY - 1),
                Quaternion.identity,
                m_Roots.GetOrCreateChunkRoot(minX - 1, minY - 1)
            );
            m_Ctx.spawnedRoots.Add(bottomLeft);

            // Bottom-right: vertical
            var bottomRight = Object.Instantiate(
                m_Settings.FenceVertical,
                m_Roots.GridToWorld(maxX + 1, minY - 1),
                Quaternion.identity,
                m_Roots.GetOrCreateChunkRoot(maxX + 1, minY - 1)
            );
            m_Ctx.spawnedRoots.Add(bottomRight);

            // Top-left: vertical
            var topLeft = Object.Instantiate(
                m_Settings.FenceVertical,
                m_Roots.GridToWorld(minX - 1, maxY + 1),
                Quaternion.identity,
                m_Roots.GetOrCreateChunkRoot(minX - 1, maxY + 1)
            );
            m_Ctx.spawnedRoots.Add(topLeft);
            m_Ctx.topCornerRoots.Add(topLeft);

            // Top-right: vertical
            var topRight = Object.Instantiate(
                m_Settings.FenceVertical,
                m_Roots.GridToWorld(maxX + 1, maxY + 1),
                Quaternion.identity,
                m_Roots.GetOrCreateChunkRoot(maxX + 1, maxY + 1)
            );
            m_Ctx.spawnedRoots.Add(topRight);
            m_Ctx.topCornerRoots.Add(topRight);
        }

        private void SpawnHorizontalEdges(int minX, int maxX, int minY, int maxY)
        {
            for (var x = minX; x <= maxX; x++)
            {
                Object.Instantiate(
                    m_Settings.FenceHorizontal,
                    m_Roots.GridToWorld(x, minY - 1),
                    Quaternion.identity,
                    m_Roots.GetOrCreateChunkRoot(x, minY - 1)
                );

                Object.Instantiate(
                    m_Settings.FenceHorizontal,
                    m_Roots.GridToWorld(x, maxY + 1),
                    Quaternion.identity,
                    m_Roots.GetOrCreateChunkRoot(x, maxY + 1)
                );
            }
        }

        private void SpawnVerticalEdges(int minX, int maxX, int minY, int maxY)
        {
            for (var y = minY; y <= maxY; y++)
            {
                m_Ctx.spawnedRoots.Add(Object.Instantiate(
                    m_Settings.FenceVertical,
                    m_Roots.GridToWorld(minX - 1, y),
                    Quaternion.identity,
                    m_Roots.GetOrCreateChunkRoot(minX - 1, y)
                ));

                m_Ctx.spawnedRoots.Add(Object.Instantiate(
                    m_Settings.FenceVertical,
                    m_Roots.GridToWorld(maxX + 1, y),
                    Quaternion.identity,
                    m_Roots.GetOrCreateChunkRoot(maxX + 1, y)
                ));
            }
        }
    }
}
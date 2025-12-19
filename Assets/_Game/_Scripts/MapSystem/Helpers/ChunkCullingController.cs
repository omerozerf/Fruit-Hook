using _Game._Scripts.ScriptableObjects;
using UnityEngine;

namespace _Game._Scripts.MapSystem.Helpers
{
    internal sealed class ChunkCullingController
    {
        private readonly Camera m_Camera;
        private readonly ChunkRootProvider m_Roots;
        private readonly GridMapBuilderSettingsSO m_Settings;


        public ChunkCullingController(GridMapBuilderSettingsSO settings, Camera camera, ChunkRootProvider roots)
        {
            m_Settings = settings;
            m_Camera = camera;
            m_Roots = roots;
        }


        public void ForceRefreshIfEnabled()
        {
            if (!m_Settings.EnableChunkCulling)
                return;

            if (!m_Camera)
                return;

            Update(true);
        }

        public void Tick()
        {
            if (!m_Settings.EnableChunkCulling)
                return;

            if (!m_Camera)
                return;

            Update(false);
        }

        private void Update(bool force)
        {
            var chunkRoots = m_Roots.ChunkRoots;
            if (chunkRoots.Count == 0)
                return;

            // Only supports orthographic for now
            if (!m_Camera.orthographic)
                return;

            var camPos = m_Camera.transform.position;
            var halfH = m_Camera.orthographicSize;
            var halfW = halfH * m_Camera.aspect;

            var minX = camPos.x - halfW - m_Settings.CullPaddingWorld;
            var maxX = camPos.x + halfW + m_Settings.CullPaddingWorld;
            var minY = camPos.y - halfH - m_Settings.CullPaddingWorld;
            var maxY = camPos.y + halfH + m_Settings.CullPaddingWorld;

            var worldChunk = Mathf.Max(0.0001f, m_Roots.ChunkWorldSize);
            var cminX = Mathf.FloorToInt(minX / worldChunk);
            var cmaxX = Mathf.FloorToInt(maxX / worldChunk);
            var cminY = Mathf.FloorToInt(minY / worldChunk);
            var cmaxY = Mathf.FloorToInt(maxY / worldChunk);

            foreach (var kvp in chunkRoots)
            {
                var key = kvp.Key;
                var root = kvp.Value;
                if (!root)
                    continue;

                var visible = key.x >= cminX && key.x <= cmaxX && key.y >= cminY && key.y <= cmaxY;

                if (force || root.gameObject.activeSelf != visible)
                    root.gameObject.SetActive(visible);
            }
        }
    }
}
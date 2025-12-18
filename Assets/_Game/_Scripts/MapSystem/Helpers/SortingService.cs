using _Game._Scripts.ScriptableObjects;
using UnityEngine;

namespace _Game._Scripts.MapSystem.Helpers
{
    internal sealed class SortingService
    {
        private readonly BuildContext m_Ctx;
        private readonly GridMapBuilderSettingsSO m_Settings;

        public SortingService(GridMapBuilderSettingsSO settings, BuildContext ctx)
        {
            m_Settings = settings;
            m_Ctx = ctx;
        }

        public void ApplyGlobalSorting()
        {
            var order = m_Settings.StartSortingOrder;

            // 1) Sort everything EXCEPT top corners first
            for (var i = 0; i < m_Ctx.spawnedRoots.Count; i++)
            {
                var root = m_Ctx.spawnedRoots[i];
                if (!root)
                    continue;

                if (m_Ctx.topCornerRoots.Contains(root))
                    continue;

                ApplySortingForRoot(root, ref order);
            }

            // 2) Sort top corners LAST
            for (var i = 0; i < m_Ctx.topCornerRoots.Count; i++)
            {
                var root = m_Ctx.topCornerRoots[i];
                if (!root)
                    continue;

                ApplySortingForRoot(root, ref order);
            }
        }

        public void DisableTopCornerLastRenderers()
        {
            for (var i = 0; i < m_Ctx.topCornerRoots.Count; i++)
            {
                var root = m_Ctx.topCornerRoots[i];
                if (!root)
                    continue;

                DisableLastRendererInSortingTraversal(root);
            }
        }

        private void ApplySortingForRoot(Transform root, ref int globalOrder)
        {
            // Rule: for every prefab, skip its first child (index 0).
            // Then, for each remaining child (in hierarchy order), assign sortingOrder using a SINGLE global decreasing counter.
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

        private void DisableLastRendererInSortingTraversal(Transform root)
        {
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

            if (last)
                last.enabled = false;
        }
    }
}
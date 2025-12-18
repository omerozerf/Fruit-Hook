using _Game._Scripts.ScriptableObjects;
using UnityEngine;

namespace _Game._Scripts.MapSystem.Helpers
{
    internal sealed class BoundaryColliderBuilder
    {
        private readonly BuildContext m_Ctx;
        private readonly Transform m_Parent;
        private readonly GridMapBuilderSettingsSO m_Settings;

        public BoundaryColliderBuilder(Transform parent, GridMapBuilderSettingsSO settings, BuildContext ctx)
        {
            m_Parent = parent;
            m_Settings = settings;
            m_Ctx = ctx;
        }

        public void Build()
        {
            Clear();

            // Fence is built at (minX-1 .. maxX+1) and (minY-1 .. maxY+1)
            var minX = -1;
            var maxX = m_Settings.Width;
            var minY = -1;
            var maxY = m_Settings.Height;

            var cell = Mathf.Max(0.0001f, m_Settings.CellSize);

            // Bounds exactly on fence centers (no extra half-cell padding)
            var worldMinX = minX * cell;
            var worldMaxX = maxX * cell;
            var worldMinY = minY * cell;
            var worldMaxY = maxY * cell;

            var thickness = Mathf.Max(0.01f, m_Settings.BoundaryThicknessWorld);
            var width = worldMaxX - worldMinX;
            var height = worldMaxY - worldMinY;

            CreateBoundaryBox(
                "Boundary_Left",
                new Vector2(worldMinX - thickness * 0.5f, (worldMinY + worldMaxY) * 0.5f),
                new Vector2(thickness, height + thickness * 2f)
            );

            CreateBoundaryBox(
                "Boundary_Right",
                new Vector2(worldMaxX + thickness * 0.5f, (worldMinY + worldMaxY) * 0.5f),
                new Vector2(thickness, height + thickness * 2f)
            );

            CreateBoundaryBox(
                "Boundary_Bottom",
                new Vector2((worldMinX + worldMaxX) * 0.5f, worldMinY - thickness * 0.5f),
                new Vector2(width + thickness * 2f, thickness)
            );

            CreateBoundaryBox(
                "Boundary_Top",
                new Vector2((worldMinX + worldMaxX) * 0.5f, worldMaxY + thickness * 0.5f),
                new Vector2(width + thickness * 2f, thickness)
            );
        }

        public void Clear()
        {
            for (var i = 0; i < m_Ctx.boundaryObjects.Count; i++)
            {
                var go = m_Ctx.boundaryObjects[i];
                if (go)
                    Object.Destroy(go);
            }

            m_Ctx.boundaryObjects.Clear();
        }

        private void CreateBoundaryBox(string name, Vector2 center, Vector2 size)
        {
            var go = new GameObject(name);
            go.transform.SetParent(m_Parent, false);
            go.transform.position = new Vector3(center.x, center.y, 0f);

            var col = go.AddComponent<BoxCollider2D>();
            col.isTrigger = false;
            col.size = size;

            m_Ctx.boundaryObjects.Add(go);
        }
    }
}
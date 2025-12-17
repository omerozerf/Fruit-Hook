#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace _Game._Scripts.Editor
{
    public static class MaterialUsageReportEditor
    {
        [MenuItem("Tools/Playable/Report Material Usage (Open Scene)")]
        private static void ReportMaterialUsage()
        {
            var materialToUsers = new Dictionary<Material, List<string>>();

            void Add(Material mat, Object owner, string extra)
            {
                if (!mat) return;

                if (!materialToUsers.TryGetValue(mat, out var list))
                {
                    list = new List<string>();
                    materialToUsers[mat] = list;
                }

                var path = GetHierarchyPath(owner);
                list.Add(string.IsNullOrEmpty(extra) ? path : $"{path} | {extra}");
            }

            // SpriteRenderers, MeshRenderers, SkinnedMeshRenderers (BIRP)
            foreach (var r in Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None))
            {
                var mats = r.sharedMaterials;
                if (mats == null) continue;

                for (var i = 0; i < mats.Length; i++) Add(mats[i], r.gameObject, $"{r.GetType().Name}[{i}]");
            }

            // UI Graphics (Image, Text, TMP components are also Graphic-derived if using TMP UGUI)
            foreach (var g in Object.FindObjectsByType<Graphic>(FindObjectsSortMode.None))
                // Graphic.material is instanced sometimes; sharedMaterial isn't always exposed.
                // We still report whatever is assigned.
                Add(g.material, g.gameObject, $"{g.GetType().Name} (Graphic.material)");

            if (materialToUsers.Count == 0)
            {
                Debug.Log("No materials found in the open scene.");
                return;
            }

            // Print summary
            var ordered = materialToUsers
                .OrderByDescending(kvp => kvp.Value.Count)
                .ThenBy(kvp => kvp.Key.name)
                .ToList();

            Debug.Log($"[Material Usage] Unique materials in scene: {ordered.Count}");

            foreach (var kvp in ordered)
            {
                var mat = kvp.Key;
                var users = kvp.Value;

                Debug.Log(
                    $"Material: '{mat.name}' | Shader: '{mat.shader.name}' | Used by: {users.Count}\n" +
                    string.Join("\n", users.Take(50)) +
                    (users.Count > 50 ? "\n... (truncated)" : "")
                );
            }
        }

        private static string GetHierarchyPath(Object owner)
        {
            if (owner is not GameObject go)
                return owner ? owner.name : "<null>";

            var t = go.transform;
            var parts = new List<string> { t.name };

            while (t.parent)
            {
                t = t.parent;
                parts.Add(t.name);
            }

            parts.Reverse();
            return string.Join("/", parts);
        }
    }
}
#endif
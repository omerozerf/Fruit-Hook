using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Game._Scripts.MapSystem
{
    public sealed class SpriteMapBaker : MonoBehaviour
    {
        [Header("Source")]
        [SerializeField] private Transform _sourceRoot;

        [Header("Chunk Settings")]
        [SerializeField] private float _cellSize = 1f;
        [SerializeField] private int _chunkSizeInCells = 32;
        
        [Tooltip("Safety limit for 16-bit index buffers (Web builds). Keep this under 65535.")]
        [SerializeField] private int _maxVerticesPerMesh = 60000;

        [Header("UV Fix")]
        [Tooltip("Inset UVs by N pixels to reduce atlas bleeding in Web builds (Luna/WebGL). 1 is usually enough.")]
        [SerializeField] private int _uvInsetPixels = 1;

        [Header("Bake Options")]
        [Tooltip("Delays baking by N frames to allow SpriteAtlas/packed textures to be resolved on Web builds.")]
        [SerializeField] private int _bakeDelayFrames = 2;
        [SerializeField] private bool _bakeOnStart = true;
        [SerializeField] private bool _disableSourceRenderersAfterBake = true;
        [SerializeField] private bool _destroySourceObjectsAfterBake;

        [Header("Sorting")]
        [Tooltip(
            "If enabled, preserves sortingLayer + sortingOrder" +
            "by creating separate chunk meshes per sorting bucket.")]
        [SerializeField] private bool _preserveSorting = true;

        [Header("Output")]
        [SerializeField] private Transform _outputRoot;
        
        private readonly List<GameObject> m_CreatedChunks = new();

        
        private void Start()
        {
            if (_bakeOnStart)
                StartCoroutine(BakeAfterDelay());
        }

        
        private static List<SpriteRenderer> CollectSpriteRenderers(Transform root)
        {
            var result = new List<SpriteRenderer>(1024);
            root.GetComponentsInChildren(true, result);

            for (var i = result.Count - 1; i >= 0; i--)
                if (!result[i] || !result[i].sprite || !result[i].enabled)
                    result.RemoveAt(i);

            return result;
        }

        private static Dictionary<GroupKey, List<SpriteRenderer>> GroupByTextureAndSorting(
            List<SpriteRenderer> renderers)
        {
            var dict = new Dictionary<GroupKey, List<SpriteRenderer>>();

            for (var i = 0; i < renderers.Count; i++)
            {
                var sr = renderers[i];
                var key = new GroupKey(sr.sprite.texture, sr.sortingLayerID, sr.sortingOrder);

                if (!dict.TryGetValue(key, out var list))
                {
                    list = new List<SpriteRenderer>(256);
                    dict[key] = list;
                }

                list.Add(sr);
            }

            return dict;
        }

        private static Dictionary<GroupKey, List<SpriteRenderer>> GroupByTextureOnly(List<SpriteRenderer> renderers)
        {
            var dict = new Dictionary<GroupKey, List<SpriteRenderer>>();

            for (var i = 0; i < renderers.Count; i++)
            {
                var sr = renderers[i];
                // Sorting bucket ignored: set to defaults so everything merges by texture
                var key = new GroupKey(sr.sprite.texture, 0, 0);

                if (!dict.TryGetValue(key, out var list))
                {
                    list = new List<SpriteRenderer>(1024);
                    dict[key] = list;
                }

                list.Add(sr);
            }

            return dict;
        }

        private static Material CreateSpriteMaterial(Texture2D texture)
        {
            var shader = Shader.Find("Sprites/Default");
            var mat = new Material(shader)
            {
                name = $"Baked_SpritesDefault_{texture.name}"
            };
            mat.mainTexture = texture;
            return mat;
        }
        
        private static void AddSimpleQuad(
            Sprite sprite,
            Matrix4x4 world,
            List<Vector3> vertices,
            List<Vector2> uvs,
            List<int> triangles,
            int uvInsetPixels)
        {
            GetQuadFromSpriteBounds(sprite, out var bl, out var tl, out var tr, out var br);
            GetQuadUVFromSpriteRect(sprite, uvInsetPixels, out var uvBl, out var uvTl, out var uvTr, out var uvBr);

            var baseIndex = vertices.Count;

            vertices.Add(world.MultiplyPoint3x4(bl));
            vertices.Add(world.MultiplyPoint3x4(tl));
            vertices.Add(world.MultiplyPoint3x4(tr));
            vertices.Add(world.MultiplyPoint3x4(br));

            uvs.Add(uvBl);
            uvs.Add(uvTl);
            uvs.Add(uvTr);
            uvs.Add(uvBr);

            triangles.Add(baseIndex + 0);
            triangles.Add(baseIndex + 1);
            triangles.Add(baseIndex + 2);
            triangles.Add(baseIndex + 0);
            triangles.Add(baseIndex + 2);
            triangles.Add(baseIndex + 3);
        }

        private static void AddSliced9Patch(
            Sprite sprite,
            Vector2 targetSize,
            Matrix4x4 world,
            List<Vector3> vertices,
            List<Vector2> uvs,
            List<int> triangles,
            int uvInsetPixels)
        {
            // Compute local rect based on Sprite pivot (pivot point should be at local (0,0)).
            var rectPx = sprite.rect;
            var pivotPx = sprite.pivot;

            var pivotNormX = rectPx.width > 0f ? pivotPx.x / rectPx.width : 0.5f;
            var pivotNormY = rectPx.height > 0f ? pivotPx.y / rectPx.height : 0.5f;

            var left = -pivotNormX * targetSize.x;
            var right = (1f - pivotNormX) * targetSize.x;
            var bottom = -pivotNormY * targetSize.y;
            var top = (1f - pivotNormY) * targetSize.y;

            // Borders are in pixels; convert to units.
            var ppu = sprite.pixelsPerUnit;
            var borderPx = sprite.border; // (left, bottom, right, top) in pixels

            var borderLeft = borderPx.x / ppu;
            var borderBottom = borderPx.y / ppu;
            var borderRight = borderPx.z / ppu;
            var borderTop = borderPx.w / ppu;

            // If targetSize is smaller than total borders, scale borders down proportionally.
            var width = targetSize.x;
            var height = targetSize.y;

            var totalBorderX = borderLeft + borderRight;
            if (totalBorderX > width && totalBorderX > 0f)
            {
                var f = width / totalBorderX;
                borderLeft *= f;
                borderRight *= f;
            }

            var totalBorderY = borderBottom + borderTop;
            if (totalBorderY > height && totalBorderY > 0f)
            {
                var f = height / totalBorderY;
                borderBottom *= f;
                borderTop *= f;
            }

            // 4x4 grid positions (local)
            var x0 = left;
            var x1 = left + borderLeft;
            var x2 = right - borderRight;
            var x3 = right;

            var y0 = bottom;
            var y1 = bottom + borderBottom;
            var y2 = top - borderTop;
            var y3 = top;

            // UVs from textureRect (atlas-safe)
            var tex = sprite.texture;
            var tr = sprite.textureRect; // in texture pixels

            var u0 = tr.xMin / tex.width;
            var u3 = tr.xMax / tex.width;
            var v0 = tr.yMin / tex.height;
            var v3 = tr.yMax / tex.height;

            // Inset outer UVs to reduce atlas bleeding on Web builds.
            var insetU = Mathf.Clamp(uvInsetPixels, 0, 8) / (float)tex.width;
            var insetV = Mathf.Clamp(uvInsetPixels, 0, 8) / (float)tex.height;

            u0 += insetU;
            u3 -= insetU;
            v0 += insetV;
            v3 -= insetV;

            var u1Raw = (tr.xMin + borderPx.x) / tex.width;
            var u2Raw = (tr.xMax - borderPx.z) / tex.width;
            var v1Raw = (tr.yMin + borderPx.y) / tex.height;
            var v2Raw = (tr.yMax - borderPx.w) / tex.height;

            // Also inset inner splits slightly and clamp into the inset outer range.
            var u1 = Mathf.Clamp(u1Raw + insetU, u0, u3);
            var u2 = Mathf.Clamp(u2Raw - insetU, u0, u3);
            var v1 = Mathf.Clamp(v1Raw + insetV, v0, v3);
            var v2 = Mathf.Clamp(v2Raw - insetV, v0, v3);

            var xs = new[] { x0, x1, x2, x3 };
            var ys = new[] { y0, y1, y2, y3 };
            var us = new[] { u0, u1, u2, u3 };
            var vs = new[] { v0, v1, v2, v3 };

            var baseIndex = vertices.Count;

            // Add 16 verts in row-major order (y from 0..3, x from 0..3)
            for (var yi = 0; yi < 4; yi++)
            for (var xi = 0; xi < 4; xi++)
            {
                var local = new Vector3(xs[xi], ys[yi], 0f);
                vertices.Add(world.MultiplyPoint3x4(local));
                uvs.Add(new Vector2(us[xi], vs[yi]));
            }

            // Add triangles for 3x3 quads
            // Vertex index helper: idx = baseIndex + yi*4 + xi
            for (var yi = 0; yi < 3; yi++)
            for (var xi = 0; xi < 3; xi++)
            {
                var i0 = baseIndex + yi * 4 + xi;
                var i1 = baseIndex + (yi + 1) * 4 + xi;
                var i2 = baseIndex + (yi + 1) * 4 + xi + 1;
                var i3 = baseIndex + yi * 4 + xi + 1;

                triangles.Add(i0);
                triangles.Add(i1);
                triangles.Add(i2);

                triangles.Add(i0);
                triangles.Add(i2);
                triangles.Add(i3);
            }
        }

        private static void GetQuadFromSpriteBounds(Sprite sprite, out Vector3 bl, out Vector3 tl, out Vector3 tr,
            out Vector3 br)
        {
            var b = sprite.bounds;
            var min = b.min;
            var max = b.max;

            bl = new Vector3(min.x, min.y, 0f);
            tl = new Vector3(min.x, max.y, 0f);
            tr = new Vector3(max.x, max.y, 0f);
            br = new Vector3(max.x, min.y, 0f);
        }

        private static void GetQuadUVFromSpriteRect(Sprite sprite, int uvInsetPixels, out Vector2 bl, out Vector2 tl,
            out Vector2 tr,
            out Vector2 br)
        {
            var tex = sprite.texture;
            var r = sprite.textureRect;

            var insetU = Mathf.Clamp(uvInsetPixels, 0, 8) / (float)tex.width;
            var insetV = Mathf.Clamp(uvInsetPixels, 0, 8) / (float)tex.height;

            var xMin = r.xMin / tex.width + insetU;
            var xMax = r.xMax / tex.width - insetU;
            var yMin = r.yMin / tex.height + insetV;
            var yMax = r.yMax / tex.height - insetV;

            // Ensure we don't invert for tiny sprites.
            if (xMax < xMin) xMax = xMin;
            if (yMax < yMin) yMax = yMin;

            bl = new Vector2(xMin, yMin);
            tl = new Vector2(xMin, yMax);
            tr = new Vector2(xMax, yMax);
            br = new Vector2(xMax, yMin);
        }
        
        private static int EstimateVertexCount(SpriteRenderer sr)
        {
            // Matches what we generate in BuildMesh:
            // Simple -> 4, Sliced -> 16, Tiled -> (fallback to Simple) 4.
            return sr.drawMode switch
            {
                SpriteDrawMode.Sliced => 16,
                var _ => 4
            };
        }

        private static List<List<SpriteRenderer>> SplitByVertexBudget(List<SpriteRenderer> renderers,
            int maxVerticesPerMesh)
        {
            var result = new List<List<SpriteRenderer>>();

            var current = new List<SpriteRenderer>(256);
            var currentVerts = 0;

            for (var i = 0; i < renderers.Count; i++)
            {
                var sr = renderers[i];
                if (!sr || !sr.sprite)
                    continue;

                var addVerts = EstimateVertexCount(sr);

                // If this renderer would exceed the budget, start a new part.
                if (current.Count > 0 && currentVerts + addVerts > maxVerticesPerMesh)
                {
                    result.Add(current);
                    current = new List<SpriteRenderer>(256);
                    currentVerts = 0;
                }

                current.Add(sr);
                currentVerts += addVerts;

                // Edge case: if we exactly hit/exceed the budget, flush current.
                if (currentVerts >= maxVerticesPerMesh)
                {
                    result.Add(current);
                    current = new List<SpriteRenderer>(256);
                    currentVerts = 0;
                }
            }

            if (current.Count > 0)
                result.Add(current);

            return result;
        }
        
        
        private IEnumerator BakeAfterDelay()
        {
            var frames = Mathf.Clamp(_bakeDelayFrames, 0, 30);
            for (var i = 0; i < frames; i++)
                yield return null;

            // One extra yield at end of frame can help on Web builds.
            yield return new WaitForEndOfFrame();

            Bake();
        }
        
        private void ValidateReferences()
        {
            if (!_sourceRoot)
                throw new InvalidOperationException("[SpriteMapBaker] Source Root is null.");

            if (!_outputRoot)
            {
                var go = new GameObject("BakedSpriteChunks");
                _outputRoot = go.transform;
                _outputRoot.SetParent(transform, false);
            }

            if (_cellSize <= 0f) _cellSize = 1f;
            if (_chunkSizeInCells <= 0) _chunkSizeInCells = 32;
        }

        private void ClearPreviousBake()
        {
            for (var i = m_CreatedChunks.Count - 1; i >= 0; i--)
                if (m_CreatedChunks[i])
                    Destroy(m_CreatedChunks[i]);

            m_CreatedChunks.Clear();

            for (var i = _outputRoot.childCount - 1; i >= 0; i--)
                Destroy(_outputRoot.GetChild(i).gameObject);
        }
        
        private void BakeGroupToChunks(List<SpriteRenderer> renderers, Material material, int sortingLayerId,
            int sortingOrder)
        {
            GetCellBounds(renderers, out var minCellX, out var minCellY, out var maxCellX, out var maxCellY);

            var chunkSize = _chunkSizeInCells;

            for (var cy = minCellY; cy <= maxCellY; cy += chunkSize)
            for (var cx = minCellX; cx <= maxCellX; cx += chunkSize)
            {
                var chunkRenderers = CollectChunkRenderers(renderers, cx, cy, chunkSize);
                if (chunkRenderers.Count == 0)
                    continue;

                CreateChunkMesh(chunkRenderers, material, cx, cy, sortingLayerId, sortingOrder);
            }
        }

        private void GetCellBounds(List<SpriteRenderer> renderers, out int minX, out int minY, out int maxX,
            out int maxY)
        {
            minX = int.MaxValue;
            minY = int.MaxValue;
            maxX = int.MinValue;
            maxY = int.MinValue;

            for (var i = 0; i < renderers.Count; i++)
            {
                var pos = renderers[i].transform.position;
                var cx = WorldToCell(pos.x);
                var cy = WorldToCell(pos.y);

                if (cx < minX) minX = cx;
                if (cy < minY) minY = cy;
                if (cx > maxX) maxX = cx;
                if (cy > maxY) maxY = cy;
            }
        }

        private List<SpriteRenderer> CollectChunkRenderers(List<SpriteRenderer> renderers, int chunkMinX, int chunkMinY,
            int chunkSize)
        {
            var list = new List<SpriteRenderer>(256);
            var chunkMaxX = chunkMinX + chunkSize - 1;
            var chunkMaxY = chunkMinY + chunkSize - 1;

            for (var i = 0; i < renderers.Count; i++)
            {
                var sr = renderers[i];
                var pos = sr.transform.position;

                var cx = WorldToCell(pos.x);
                var cy = WorldToCell(pos.y);

                if (cx < chunkMinX || cx > chunkMaxX) continue;
                if (cy < chunkMinY || cy > chunkMaxY) continue;

                list.Add(sr);
            }

            return list;
        }

        private void CreateChunkMesh(List<SpriteRenderer> chunkRenderers, Material material, int chunkMinX,
            int chunkMinY, int sortingLayerId, int sortingOrder)
        {
            // Split into multiple meshes to stay within 16-bit index limits (important for Web/Luna).
            var parts = SplitByVertexBudget(chunkRenderers, Mathf.Clamp(_maxVerticesPerMesh, 1000, 65000));

            for (var p = 0; p < parts.Count; p++)
            {
                var partRenderers = parts[p];
                if (partRenderers.Count == 0)
                    continue;

                var chunkGo = new GameObject($"Chunk_{chunkMinX}_{chunkMinY}_SO{sortingOrder}_P{p}");
                chunkGo.transform.SetParent(_outputRoot, false);
                m_CreatedChunks.Add(chunkGo);

                var mf = chunkGo.AddComponent<MeshFilter>();
                var mr = chunkGo.AddComponent<MeshRenderer>();
                mr.sharedMaterial = material;

                mr.sortingLayerID = sortingLayerId;
                mr.sortingOrder = sortingOrder;

                var mesh = BuildMesh(partRenderers);
                mf.sharedMesh = mesh;
            }
        }

        private Mesh BuildMesh(List<SpriteRenderer> renderers)
        {
            // Some SpriteRenderers may be Simple (4 verts) while others may be Sliced (16 verts).
            // Use lists to support variable vertex counts.
            var vertices = new List<Vector3>(renderers.Count * 4);
            var uvs = new List<Vector2>(renderers.Count * 4);
            var triangles = new List<int>(renderers.Count * 6);

            for (var i = 0; i < renderers.Count; i++)
            {
                var sr = renderers[i];
                var sprite = sr.sprite;

                var world = sr.transform.localToWorldMatrix;

                switch (sr.drawMode)
                {
                    case SpriteDrawMode.Simple:
                        AddSimpleQuad(sprite, world, vertices, uvs, triangles, _uvInsetPixels);
                        break;

                    case SpriteDrawMode.Sliced:
                        AddSliced9Patch(sprite, sr.size, world, vertices, uvs, triangles, _uvInsetPixels);
                        break;

                    case SpriteDrawMode.Tiled:
                        // Tiled is more complex (repeats center area). For bake purposes, fall back to Simple.
                        // If you need perfect tiling, we can add a tiled implementation next.
                        AddSimpleQuad(sprite, world, vertices, uvs, triangles, _uvInsetPixels);
                        break;

                    default:
                        AddSimpleQuad(sprite, world, vertices, uvs, triangles, _uvInsetPixels);
                        break;
                }
            }

            var mesh = new Mesh { name = "BakedSpriteChunkMesh" };

            mesh.SetVertices(vertices);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(triangles, 0, true);
            mesh.RecalculateBounds();
            return mesh;
        }
        
        private int WorldToCell(float world)
        {
            // Statik grid için Round genelde ok; eğer edge-case varsa Floor’a alırız.
            return Mathf.RoundToInt(world / _cellSize);
        }

        private void PostBakeCleanup(List<SpriteRenderer> renderers)
        {
            if (_disableSourceRenderersAfterBake)
                for (var i = 0; i < renderers.Count; i++)
                    if (renderers[i])
                        renderers[i].enabled = false;

            if (_destroySourceObjectsAfterBake)
                for (var i = _sourceRoot.childCount - 1; i >= 0; i--)
                    Destroy(_sourceRoot.GetChild(i).gameObject);
        }
        
        
        [ContextMenu("Bake")]
        public void Bake()
        {
            ValidateReferences();
            ClearPreviousBake();

            var renderers = CollectSpriteRenderers(_sourceRoot);
            if (renderers.Count == 0)
            {
                Debug.LogWarning("[SpriteMapBaker] No SpriteRenderers found under source root.");
                return;
            }

            // Safety: if any sprite texture is missing/invalid, do NOT disable sources.
            // This prevents ending up with baked meshes sampling a placeholder texture on Web builds.
            var hasInvalidTexture = false;
            for (var i = 0; i < renderers.Count; i++)
            {
                var sp = renderers[i].sprite;
                var tx = sp ? sp.texture : null;
                if (!tx || tx.width <= 0 || tx.height <= 0)
                {
                    hasInvalidTexture = true;
                    break;
                }
            }

            if (hasInvalidTexture)
            {
                Debug.LogWarning(
                    "[SpriteMapBaker] Bake aborted: detected invalid sprite textures (likely atlas not ready). Retrying next frame.");
                StartCoroutine(BakeAfterDelay());
                return;
            }

            // Group order:
            // - always by texture (to avoid texture switches)
            // - optionally by sorting bucket (to preserve visual order)
            var groups = _preserveSorting
                ? GroupByTextureAndSorting(renderers)
                : GroupByTextureOnly(renderers);

            foreach (var group in groups)
            {
                var tex = group.Key.texture;
                var mat = CreateSpriteMaterial(tex);

                BakeGroupToChunks(
                    group.Value,
                    mat,
                    group.Key.sortingLayerId,
                    group.Key.sortingOrder
                );
            }

            PostBakeCleanup(renderers);

            Debug.Log(
                $"[SpriteMapBaker] Bake complete. Source SR: {renderers.Count} | Chunks: {m_CreatedChunks.Count} | PreserveSorting: {_preserveSorting}");
        }

        
        private readonly struct GroupKey : IEquatable<GroupKey>
        {
            public readonly Texture2D texture;
            public readonly int sortingLayerId;
            public readonly int sortingOrder;

            public GroupKey(Texture2D texture, int sortingLayerId, int sortingOrder)
            {
                this.texture = texture;
                this.sortingLayerId = sortingLayerId;
                this.sortingOrder = sortingOrder;
            }

            public bool Equals(GroupKey other)
            {
                return texture == other.texture &&
                       sortingLayerId == other.sortingLayerId &&
                       sortingOrder == other.sortingOrder;
            }

            public override bool Equals(object obj)
            {
                return obj is GroupKey other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hash = 17;
                    hash = hash * 31 + (texture ? texture.GetHashCode() : 0);
                    hash = hash * 31 + sortingLayerId.GetHashCode();
                    hash = hash * 31 + sortingOrder.GetHashCode();
                    return hash;
                }
            }
        }
    }
}
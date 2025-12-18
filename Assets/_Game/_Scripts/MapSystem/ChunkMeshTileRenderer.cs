using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game._Scripts.MapSystem
{
    public sealed class ChunkMeshTileRenderer : MonoBehaviour
    {
        [SerializeField] private Sprite[] _tileSprites;
        
        [Header("Grid")]
        [SerializeField] private float _cellSize = 1f;
        [SerializeField] private int _width = 64;
        [SerializeField] private int _height = 64;

        [Header("Chunking")]
        [SerializeField] private int _chunkSize = 32;
        [SerializeField] private int _maxVerticesPerMesh = 60000;

        [Header("Random Generation")]
        [SerializeField] private int[] _tileWeights;
        [Min(0)] [SerializeField] private int _emptyTileWeight = 0;
        [SerializeField] private bool _useRandomSeed;
        [SerializeField] private int _randomSeed = 12345;

        [Header("Material")]
        [SerializeField] private Material _sharedMaterial;
        [SerializeField] private bool _allowMultipleTextures = false;

        [Header("Sorting")]
        [SerializeField] private int _sortingLayerId;
        [SerializeField] private int _sortingOrder = 0;

        [Header("Culling (Optional)")]
        [SerializeField] private bool _enableChunkCulling = true;
        [SerializeField] private Camera _cullCamera;
        [SerializeField] private float _cullPaddingWorld = 2f;

        [Header("Lifecycle")]
        [SerializeField] private bool _buildOnAwake = true;

        private int[,] m_Tiles; // [x,y] => tileId
        private readonly List<Chunk> m_Chunks = new(256);
        private Texture2D m_MainTexture;
        private readonly Dictionary<Texture2D, Material> m_TextureMaterials = new(8);

        
        private void Awake()
        {
            if (_buildOnAwake)
                RebuildAll();
        }

        private void Update()
        {
            if (_enableChunkCulling)
                UpdateChunkCulling();
        }

        [ContextMenu("Rebuild All")]
        public void RebuildAll()
        {
            ValidateInputs();
            LoadTilesFromCsv();
            ClearChunks();
            ClearMaterialCache();
            BuildChunks();
            ApplyMaterialTextureOnce();
            BuildAllChunkMeshes();
            UpdateChunkCulling();
        }
        
        public void SetTile(int x, int y, int tileId)
        {
            if (!IsInBounds(x, y))
                return;

            m_Tiles[x, y] = tileId;

            var cx = x / _chunkSize;
            var cy = y / _chunkSize;
            var chunk = FindChunk(cx, cy);
            if (chunk == null)
                return;

            BuildChunkMesh(chunk);
        }

        private void ValidateInputs()
        {
            if (_cellSize <= 0f) _cellSize = 1f;
            if (_chunkSize <= 0) _chunkSize = 32;
            if (_width <= 0) _width = 1;
            if (_height <= 0) _height = 1;

            if (_tileSprites == null || _tileSprites.Length == 0)
                throw new InvalidOperationException("[ChunkMeshTileRenderer] Tile sprites not assigned.");

            if (_sharedMaterial == null)
            {
                var shader = Shader.Find("Sprites/Default");
                _sharedMaterial = new Material(shader) { name = "ChunkMeshTileRenderer_Mat" };
            }

            if (_enableChunkCulling && !_cullCamera)
                _cullCamera = Camera.main;

            ValidateSpriteTextures();
        }

        private void ValidateSpriteTextures()
        {
            var texToIndices = new Dictionary<Texture2D, List<int>>();

            for (var i = 0; i < _tileSprites.Length; i++)
            {
                var sp = _tileSprites[i];
                if (!sp || !sp.texture)
                    continue;

                if (!texToIndices.TryGetValue(sp.texture, out var list))
                {
                    list = new List<int>();
                    texToIndices.Add(sp.texture, list);
                }

                list.Add(i);
            }

            if (texToIndices.Count <= 1)
                return;

            // Build readable message: textureName => indices
            var msg = "[ChunkMeshTileRenderer] Multiple distinct textures detected in _tileSprites. " +
                      "This renderer assumes a single atlas/texture. " +
                      "Either put all sprites into ONE sliced texture, or enable _allowMultipleTextures (visuals may break).\n";

            foreach (var kv in texToIndices)
            {
                var t = kv.Key;
                var indices = kv.Value;
                msg += $"- Texture '{t.name}' ({t.width}x{t.height}) : IDs [{string.Join(",", indices)}]\n";
            }

            Debug.LogError(msg, this);
        }

        private void LoadTilesFromCsv()
        {
            // CSV devre dışı: her rebuild'te weight'lere göre random dağıt.
            m_Tiles = new int[_width, _height];
            GenerateRandomTilesWeighted();
        }
        

        private void GenerateRandomTilesWeighted()
        {
            if (_tileSprites == null || _tileSprites.Length == 0)
                return;

            if (_useRandomSeed)
                UnityEngine.Random.InitState(_randomSeed);

            // Precompute valid tile ids and weights.
            var validIds = new List<int>(_tileSprites.Length);
            var validWeights = new List<int>(_tileSprites.Length);

            var useWeightsArray = _tileWeights != null && _tileWeights.Length == _tileSprites.Length;

            var totalWeight = 0;
            for (var i = 0; i < _tileSprites.Length; i++)
            {
                var sp = _tileSprites[i];
                if (!sp)
                    continue;

                var w = useWeightsArray ? _tileWeights[i] : 1;
                if (w <= 0)
                    continue;

                validIds.Add(i);
                validWeights.Add(w);
                totalWeight += w;
            }

            // Add optional empty tiles.
            if (_emptyTileWeight > 0)
                totalWeight += _emptyTileWeight;

            if (totalWeight <= 0)
            {
                Debug.LogError("[ChunkMeshTileRenderer] All tile weights are 0 or sprites are missing.", this);
                return;
            }

            for (var y = 0; y < _height; y++)
            for (var x = 0; x < _width; x++)
                m_Tiles[x, y] = PickWeightedTileId(validIds, validWeights, totalWeight);
        }

        private int PickWeightedTileId(List<int> validIds, List<int> validWeights, int totalWeight)
        {
            var roll = UnityEngine.Random.Range(0, totalWeight);

            if (_emptyTileWeight > 0)
            {
                roll -= _emptyTileWeight;
                if (roll < 0)
                    return -1;
            }

            for (var i = 0; i < validIds.Count; i++)
            {
                roll -= validWeights[i];
                if (roll < 0)
                    return validIds[i];
            }

            return validIds.Count > 0 ? validIds[0] : -1;
        }

        private void ClearChunks()
        {
            for (var i = 0; i < m_Chunks.Count; i++)
                m_Chunks[i].Destroy();

            m_Chunks.Clear();
        }

        private void ClearMaterialCache()
        {
            // Destroy runtime-created materials to avoid leaks in editor play mode.
            foreach (var kv in m_TextureMaterials)
            {
                var mat = kv.Value;
                if (mat)
                    Destroy(mat);
            }

            m_TextureMaterials.Clear();
        }

        private void BuildChunks()
        {
            var chunkCountX = Mathf.CeilToInt(_width / (float)_chunkSize);
            var chunkCountY = Mathf.CeilToInt(_height / (float)_chunkSize);

            for (var cy = 0; cy < chunkCountY; cy++)
            for (var cx = 0; cx < chunkCountX; cx++)
            {
                var chunkGo = new GameObject($"Chunk_{cx}_{cy}");
                chunkGo.transform.SetParent(transform, false);

                var chunk = new Chunk(cx, cy, chunkGo);
                m_Chunks.Add(chunk);
            }
        }

        private void ApplyMaterialTextureOnce()
        {
            
            m_MainTexture = FindFirstValidTexture();
            if (!m_MainTexture)
            {
                Debug.LogError("[ChunkMeshTileRenderer] Could not find a valid sprite texture.", this);
                return;
            }

            _sharedMaterial.mainTexture = m_MainTexture;
        }

        private Texture2D FindFirstValidTexture()
        {
            for (var i = 0; i < _tileSprites.Length; i++)
            {
                var sp = _tileSprites[i];
                if (sp && sp.texture)
                    return sp.texture;
            }

            return null;
        }

        private void BuildAllChunkMeshes()
        {
            for (var i = 0; i < m_Chunks.Count; i++)
                BuildChunkMesh(m_Chunks[i]);
        }

        private void BuildChunkMesh(Chunk chunk)
        {
            var minX = chunk.ChunkX * _chunkSize;
            var minY = chunk.ChunkY * _chunkSize;

            var maxX = Mathf.Min(minX + _chunkSize, _width);
            var maxY = Mathf.Min(minY + _chunkSize, _height);

            // Detect whether we have multiple textures in the sprite list.
            var hasMultipleTextures = HasMultipleSpriteTextures();

            if (!hasMultipleTextures)
            {
                // Single-texture fast path
                var vertices = new List<Vector3>(4096);
                var uvs = new List<Vector2>(4096);
                var triangles = new List<int>(6144);

                for (var y = minY; y < maxY; y++)
                for (var x = minX; x < maxX; x++)
                {
                    var id = m_Tiles[x, y];
                    if (id < 0)
                        continue;

                    var sprite = GetSpriteForId(id);
                    if (!sprite)
                        continue;

                    if (vertices.Count + 4 > Mathf.Clamp(_maxVerticesPerMesh, 1000, 65000))
                        break;

                    AddTileQuad(sprite, x, y, vertices, uvs, triangles);
                }

                var mesh = new Mesh { name = $"ChunkMesh_{chunk.ChunkX}_{chunk.ChunkY}_Main" };
                mesh.SetVertices(vertices);
                mesh.SetUVs(0, uvs);
                mesh.SetTriangles(triangles, 0, true);
                mesh.RecalculateBounds();

                var sub = chunk.GetOrCreateSubRenderer(chunk.GameObject.transform, "Main", _sharedMaterial,
                    _sortingLayerId, _sortingOrder);
                sub.MeshFilter.sharedMesh = mesh;

                // Disable any other sub renderers
                chunk.DisableAllExcept("Main");

                chunk.UpdateWorldBounds(mesh.bounds, sub.GameObject.transform);
                return;
            }

            // Multi-texture path: build one mesh per texture
            var buckets = new Dictionary<Texture2D, MeshBuildBucket>(8);

            for (var y = minY; y < maxY; y++)
            for (var x = minX; x < maxX; x++)
            {
                var id = m_Tiles[x, y];
                if (id < 0)
                    continue;

                var sprite = GetSpriteForId(id);
                if (!sprite || !sprite.texture)
                    continue;

                if (!buckets.TryGetValue(sprite.texture, out var bucket))
                {
                    bucket = new MeshBuildBucket();
                    buckets.Add(sprite.texture, bucket);
                }

                if (bucket.vertices.Count + 4 > Mathf.Clamp(_maxVerticesPerMesh, 1000, 65000))
                    continue;

                AddTileQuad(sprite, x, y, bucket.vertices, bucket.uvs, bucket.triangles);
            }

            // Build meshes and assign
            Bounds? combinedLocalBounds = null;

            foreach (var kv in buckets)
            {
                var tex = kv.Key;
                var b = kv.Value;

                if (b.vertices.Count == 0)
                    continue;

                var mesh = new Mesh { name = $"ChunkMesh_{chunk.ChunkX}_{chunk.ChunkY}_{tex.name}" };
                mesh.SetVertices(b.vertices);
                mesh.SetUVs(0, b.uvs);
                mesh.SetTriangles(b.triangles, 0, true);
                mesh.RecalculateBounds();

                var mat = GetOrCreateMaterialForTexture(tex);
                var childName = $"Tex_{tex.name}";
                var sub = chunk.GetOrCreateSubRenderer(chunk.GameObject.transform, childName, mat, _sortingLayerId,
                    _sortingOrder);
                sub.MeshFilter.sharedMesh = mesh;
                sub.GameObject.SetActive(true);

                combinedLocalBounds = combinedLocalBounds.HasValue
                    ? Encapsulate(combinedLocalBounds.Value, mesh.bounds)
                    : mesh.bounds;
            }

            // Disable unused sub renderers (those not in buckets)
            chunk.DisableAllExcept(buckets);

            // Update combined world bounds for culling
            if (combinedLocalBounds.HasValue)
                chunk.UpdateWorldBounds(combinedLocalBounds.Value, chunk.GameObject.transform);
            else
                chunk.UpdateWorldBounds(new Bounds(Vector3.zero, Vector3.zero), chunk.GameObject.transform);
        }

        private bool HasMultipleSpriteTextures()
        {
            Texture2D first = null;
            for (var i = 0; i < _tileSprites.Length; i++)
            {
                var sp = _tileSprites[i];
                if (!sp || !sp.texture)
                    continue;

                if (!first)
                    first = sp.texture;
                else if (sp.texture != first)
                    return true;
            }

            return false;
        }

        private Material GetOrCreateMaterialForTexture(Texture2D tex)
        {
            if (!tex)
                return _sharedMaterial;

            if (m_TextureMaterials.TryGetValue(tex, out var existing) && existing)
                return existing;

            var shader = Shader.Find("Sprites/Default");
            var mat = new Material(shader)
            {
                name = $"ChunkMeshTileRenderer_Mat_{tex.name}",
                mainTexture = tex
            };

            m_TextureMaterials[tex] = mat;
            return mat;
        }

        private static Bounds Encapsulate(Bounds a, Bounds b)
        {
            a.Encapsulate(b.min);
            a.Encapsulate(b.max);
            return a;
        }

        private sealed class MeshBuildBucket
        {
            public readonly List<Vector3> vertices = new(4096);
            public readonly List<Vector2> uvs = new(4096);
            public readonly List<int> triangles = new(6144);
        }

        private Sprite GetSpriteForId(int tileId)
        {
            if (tileId < 0 || tileId >= _tileSprites.Length)
                return null;
            return _tileSprites[tileId];
        }

        private void AddTileQuad(
            Sprite sprite,
            int gridX,
            int gridY,
            List<Vector3> vertices,
            List<Vector2> uvs,
            List<int> triangles)
        {
            var baseIndex = vertices.Count;

            var x0 = gridX * _cellSize;
            var y0 = gridY * _cellSize;
            var x1 = x0 + _cellSize;
            var y1 = y0 + _cellSize;

            vertices.Add(new Vector3(x0, y0, 0f));
            vertices.Add(new Vector3(x0, y1, 0f));
            vertices.Add(new Vector3(x1, y1, 0f));
            vertices.Add(new Vector3(x1, y0, 0f));

            var tex = sprite.texture;
            var tr = sprite.textureRect;

            var u0 = tr.xMin / tex.width;
            var u1 = tr.xMax / tex.width;
            var v0 = tr.yMin / tex.height;
            var v1 = tr.yMax / tex.height;

            uvs.Add(new Vector2(u0, v0));
            uvs.Add(new Vector2(u0, v1));
            uvs.Add(new Vector2(u1, v1));
            uvs.Add(new Vector2(u1, v0));

            triangles.Add(baseIndex + 0);
            triangles.Add(baseIndex + 1);
            triangles.Add(baseIndex + 2);
            triangles.Add(baseIndex + 0);
            triangles.Add(baseIndex + 2);
            triangles.Add(baseIndex + 3);
        }

        private void UpdateChunkCulling()
        {
            if (!_enableChunkCulling || !_cullCamera)
                return;

            var cam = _cullCamera;
            var z = Mathf.Abs(cam.transform.position.z);

            var min = cam.ViewportToWorldPoint(new Vector3(0f, 0f, z));
            var max = cam.ViewportToWorldPoint(new Vector3(1f, 1f, z));

            var rect = new Rect(
                min.x - _cullPaddingWorld,
                min.y - _cullPaddingWorld,
                (max.x - min.x) + _cullPaddingWorld * 2f,
                (max.y - min.y) + _cullPaddingWorld * 2f
            );

            for (var i = 0; i < m_Chunks.Count; i++)
            {
                var c = m_Chunks[i];
                var b = c.WorldBounds;

                var visible =
                    b.max.x >= rect.xMin &&
                    b.min.x <= rect.xMax &&
                    b.max.y >= rect.yMin &&
                    b.min.y <= rect.yMax;

                if (c.GameObject.activeSelf != visible)
                    c.GameObject.SetActive(visible);
            }
        }

        private bool IsInBounds(int x, int y)
        {
            return x >= 0 && y >= 0 && x < _width && y < _height;
        }

        private Chunk FindChunk(int chunkX, int chunkY)
        {
            for (var i = 0; i < m_Chunks.Count; i++)
            {
                var c = m_Chunks[i];
                if (c.ChunkX == chunkX && c.ChunkY == chunkY)
                    return c;
            }

            return null;
        }

        
        [Serializable]
        private sealed class Chunk
        {
            public int ChunkX => m_ChunkX;
            public int ChunkY => m_ChunkY;
            public GameObject GameObject => m_Go;
            public Bounds WorldBounds => m_WorldBounds;

            private readonly int m_ChunkX;
            private readonly int m_ChunkY;
            private readonly GameObject m_Go;

            private Bounds m_WorldBounds;

            private readonly Dictionary<string, SubRenderer> m_Sub = new(8);

            public Chunk(int chunkX, int chunkY, GameObject go)
            {
                m_ChunkX = chunkX;
                m_ChunkY = chunkY;
                m_Go = go;
                m_WorldBounds = new Bounds(go.transform.position, Vector3.zero);
            }

            public SubRenderer GetOrCreateSubRenderer(Transform parent, string name, Material mat, int sortingLayerId,
                int sortingOrder)
            {
                if (m_Sub.TryGetValue(name, out var existing) && existing != null)
                {
                    if (existing.MeshRenderer)
                    {
                        existing.MeshRenderer.sharedMaterial = mat;
                        existing.MeshRenderer.sortingLayerID = sortingLayerId;
                        existing.MeshRenderer.sortingOrder = sortingOrder;
                    }

                    return existing;
                }

                var go = new GameObject(name);
                go.transform.SetParent(parent, false);

                var mf = go.AddComponent<MeshFilter>();
                var mr = go.AddComponent<MeshRenderer>();
                mr.sharedMaterial = mat;
                mr.sortingLayerID = sortingLayerId;
                mr.sortingOrder = sortingOrder;

                var sub = new SubRenderer(go, mf, mr);
                m_Sub[name] = sub;
                return sub;
            }

            public void DisableAllExcept(string keepName)
            {
                foreach (var kv in m_Sub)
                {
                    if (kv.Key == keepName)
                        continue;
                    if (kv.Value?.GameObject)
                        kv.Value.GameObject.SetActive(false);
                }
            }

            public void DisableAllExcept(Dictionary<Texture2D, MeshBuildBucket> buckets)
            {
                foreach (var kv in m_Sub)
                {
                    var name = kv.Key;
                    if (name == "Main")
                    {
                        if (kv.Value?.GameObject)
                            kv.Value.GameObject.SetActive(false);
                        continue;
                    }

                    // For Tex_<name> children: keep only those present in buckets
                    if (!name.StartsWith("Tex_"))
                    {
                        if (kv.Value?.GameObject)
                            kv.Value.GameObject.SetActive(false);
                        continue;
                    }

                    var shouldKeep = false;
                    foreach (var t in buckets.Keys)
                    {
                        if (name == $"Tex_{t.name}")
                        {
                            shouldKeep = true;
                            break;
                        }
                    }

                    if (kv.Value?.GameObject)
                        kv.Value.GameObject.SetActive(shouldKeep);
                }
            }

            public void UpdateWorldBounds(Bounds localMeshBounds, Transform t)
            {
                var center = t.TransformPoint(localMeshBounds.center);
                var size = Vector3.Scale(localMeshBounds.size, t.lossyScale);
                m_WorldBounds = new Bounds(center, size);
            }

            public void Destroy()
            {
                if (m_Go)
                    UnityEngine.Object.Destroy(m_Go);
            }
        }

        [Serializable]
        private sealed class SubRenderer
        {
            public GameObject GameObject { get; }
            public MeshFilter MeshFilter { get; }
            public MeshRenderer MeshRenderer { get; }

            public SubRenderer(GameObject go, MeshFilter mf, MeshRenderer mr)
            {
                GameObject = go;
                MeshFilter = mf;
                MeshRenderer = mr;
            }
        }
    }
}